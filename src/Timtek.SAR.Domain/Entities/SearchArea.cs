using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Domain.Entities;

public sealed class SearchArea : IDomainEntity<Guid>
{
    public Guid Id { get; set; }
    public required Guid CaseId { get; set; }
    public required string Name { get; set; }
    public required SearchAreaType AreaType { get; set; }

    /// <summary>
    /// GeoJSON geometry string representing the area boundary.
    /// For Polygon: a GeoJSON Polygon. For Circle: stored as centre point + radius.
    /// </summary>
    public required string GeoJson { get; set; }

    /// <summary>
    /// For circle areas only: radius in metres from the centre point.
    /// </summary>
    public double? RadiusMetres { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<Sector> Sectors { get; set; } = [];

    public Sector AddSector(string name, string geoJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(geoJson);

        var sector = new Sector
        {
            SearchAreaId = Id,
            Name = name,
            GeoJson = geoJson,
        };
        Sectors.Add(sector);
        return sector;
    }
}
