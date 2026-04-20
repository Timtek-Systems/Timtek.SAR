using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Domain.Entities;

public sealed class Sector : IDomainEntity<Guid>
{
    static readonly Dictionary<SectorStatus, SectorStatus[]> ValidTransitions = new()
    {
        [SectorStatus.NotStarted] = [SectorStatus.InProgress],
        [SectorStatus.InProgress] = [SectorStatus.Completed, SectorStatus.NeedsResearch],
        [SectorStatus.Completed] = [SectorStatus.NeedsResearch],
        [SectorStatus.NeedsResearch] = [SectorStatus.InProgress],
    };

    public Guid Id { get; set; }
    public required Guid SearchAreaId { get; set; }
    public required string Name { get; set; }
    public required string GeoJson { get; set; }
    public SectorStatus Status { get; private set; } = SectorStatus.NotStarted;
    public Guid? AssignedToUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public void TransitionTo(SectorStatus target)
    {
        if (!ValidTransitions[Status].Contains(target))
            throw new InvalidOperationException(
                $"Cannot transition sector from {Status} to {target}.");
        Status = target;
    }

    public void StartSearch() => TransitionTo(SectorStatus.InProgress);
    public void Complete() => TransitionTo(SectorStatus.Completed);
    public void MarkNeedsResearch() => TransitionTo(SectorStatus.NeedsResearch);
}
