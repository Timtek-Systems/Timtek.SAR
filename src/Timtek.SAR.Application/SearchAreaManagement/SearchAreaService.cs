using System.Text.Json;
using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.SearchAreaManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.SearchAreaManagement;

public sealed class SearchAreaService(
    IRepository<SearchArea, Guid> searchAreaRepository,
    IRepository<Sector, Guid> sectorRepository,
    IUnitOfWork unitOfWork) : ISearchAreaService
{
    public async Task<SearchAreaDto> CreateAsync(CreateSearchAreaRequest request)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.GeoJson);

        if (request.AreaType == SearchAreaType.Circle && request.RadiusMetres is null or <= 0)
            throw new ArgumentException("Circle areas require a positive radius.");

        var area = new SearchArea
        {
            Id = Guid.NewGuid(),
            CaseId = request.CaseId,
            Name = request.Name,
            AreaType = request.AreaType,
            GeoJson = request.GeoJson,
            RadiusMetres = request.RadiusMetres,
        };

        searchAreaRepository.Add(area);
        await unitOfWork.CommitAsync();
        return MapToDto(area);
    }

    public Task<Maybe<SearchAreaDto>> GetByIdAsync(Guid searchAreaId)
    {
        var area = searchAreaRepository.GetMaybe(searchAreaId);
        return Task.FromResult(area.Any()
            ? MapToDto(area.Single()).AsMaybe()
            : Maybe<SearchAreaDto>.Empty);
    }

    public Task<IReadOnlyList<SearchAreaDto>> GetByCaseIdAsync(Guid caseId)
    {
        var areas = searchAreaRepository.GetAll()
            .Where(a => a.CaseId == caseId)
            .Select(MapToDto)
            .ToList();
        return Task.FromResult<IReadOnlyList<SearchAreaDto>>(areas);
    }

    public async Task DeleteAsync(Guid searchAreaId)
    {
        var area = searchAreaRepository.GetMaybe(searchAreaId);
        if (area.Any())
        {
            searchAreaRepository.Remove(area.Single());
            await unitOfWork.CommitAsync();
        }
    }

    public async Task<SectorDto> AddSectorAsync(Guid searchAreaId, AddSectorRequest request)
    {
        var area = searchAreaRepository.GetMaybe(searchAreaId);
        if (!area.Any())
            throw new InvalidOperationException("Search area not found.");

        ArgumentException.ThrowIfNullOrWhiteSpace(request.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.GeoJson);

        var sector = new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = searchAreaId,
            Name = request.Name,
            GeoJson = request.GeoJson,
        };
        sectorRepository.Add(sector);
        await unitOfWork.CommitAsync();
        return MapSectorToDto(sector);
    }

    public Task<Maybe<SectorDto>> GetSectorByIdAsync(Guid sectorId)
    {
        var sector = sectorRepository.GetMaybe(sectorId);
        return Task.FromResult(sector.Any()
            ? MapSectorToDto(sector.Single()).AsMaybe()
            : Maybe<SectorDto>.Empty);
    }

    public async Task<SectorDto> UpdateSectorStatusAsync(Guid sectorId, SectorStatus status)
    {
        var sector = sectorRepository.GetMaybe(sectorId);
        if (!sector.Any())
            throw new InvalidOperationException("Sector not found.");

        var entity = sector.Single();
        entity.TransitionTo(status);
        await unitOfWork.CommitAsync();
        return MapSectorToDto(entity);
    }

    public async Task<SectorDto> AssignSectorAsync(Guid sectorId, Guid? userId)
    {
        var sector = sectorRepository.GetMaybe(sectorId);
        if (!sector.Any())
            throw new InvalidOperationException("Sector not found.");

        var entity = sector.Single();
        entity.AssignedToUserId = userId;
        await unitOfWork.CommitAsync();
        return MapSectorToDto(entity);
    }

    public async Task DeleteSectorAsync(Guid sectorId)
    {
        var sector = sectorRepository.GetMaybe(sectorId);
        if (sector.Any())
        {
            sectorRepository.Remove(sector.Single());
            await unitOfWork.CommitAsync();
        }
    }

    public Task<string> ExportGeoJsonAsync(Guid searchAreaId)
    {
        var area = searchAreaRepository.GetMaybe(searchAreaId);
        if (!area.Any())
            throw new InvalidOperationException("Search area not found.");

        var searchArea = area.Single();
        var featureCollection = new
        {
            type = "FeatureCollection",
            features = new object[]
            {
                new
                {
                    type = "Feature",
                    properties = new { name = searchArea.Name, areaType = searchArea.AreaType.ToString() },
                    geometry = JsonSerializer.Deserialize<object>(searchArea.GeoJson),
                },
            }.Concat(searchArea.Sectors.Select(s => new
            {
                type = "Feature",
                properties = (object)new { name = s.Name, status = s.Status.ToString(), sectorId = s.Id },
                geometry = (object)JsonSerializer.Deserialize<object>(s.GeoJson)!,
            })).ToArray(),
        };

        return Task.FromResult(JsonSerializer.Serialize(featureCollection, new JsonSerializerOptions { WriteIndented = true }));
    }

    static SearchAreaDto MapToDto(SearchArea area) => new(
        area.Id,
        area.CaseId,
        area.Name,
        area.AreaType,
        area.GeoJson,
        area.RadiusMetres,
        area.CreatedAtUtc,
        area.Sectors.Select(MapSectorToDto).ToList());

    static SectorDto MapSectorToDto(Sector sector) => new(
        sector.Id,
        sector.SearchAreaId,
        sector.Name,
        sector.GeoJson,
        sector.Status,
        sector.AssignedToUserId,
        sector.CreatedAtUtc);
}
