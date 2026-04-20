using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.SearchAreaManagement.Dtos;

public sealed record CreateSearchAreaRequest(
    Guid CaseId,
    string Name,
    SearchAreaType AreaType,
    string GeoJson,
    double? RadiusMetres = null);

public sealed record SearchAreaDto(
    Guid Id,
    Guid CaseId,
    string Name,
    SearchAreaType AreaType,
    string GeoJson,
    double? RadiusMetres,
    DateTime CreatedAtUtc,
    IReadOnlyList<SectorDto> Sectors);

public sealed record AddSectorRequest(
    string Name,
    string GeoJson);

public sealed record SectorDto(
    Guid Id,
    Guid SearchAreaId,
    string Name,
    string GeoJson,
    SectorStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);

public sealed record UpdateSectorStatusRequest(SectorStatus Status);

public sealed record AssignSectorRequest(Guid? UserId);
