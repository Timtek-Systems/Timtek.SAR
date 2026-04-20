using TA.Utils.Core;
using Timtek.SAR.Application.SearchAreaManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.SearchAreaManagement;

public interface ISearchAreaService
{
    Task<SearchAreaDto> CreateAsync(CreateSearchAreaRequest request);
    Task<Maybe<SearchAreaDto>> GetByIdAsync(Guid searchAreaId);
    Task<IReadOnlyList<SearchAreaDto>> GetByCaseIdAsync(Guid caseId);
    Task DeleteAsync(Guid searchAreaId);
    Task<SectorDto> AddSectorAsync(Guid searchAreaId, AddSectorRequest request);
    Task<Maybe<SectorDto>> GetSectorByIdAsync(Guid sectorId);
    Task<SectorDto> UpdateSectorStatusAsync(Guid sectorId, SectorStatus status);
    Task<SectorDto> AssignSectorAsync(Guid sectorId, Guid? userId);
    Task DeleteSectorAsync(Guid sectorId);
    Task<string> ExportGeoJsonAsync(Guid searchAreaId);
}
