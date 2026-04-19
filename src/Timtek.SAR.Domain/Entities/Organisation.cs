using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Domain.Entities;

public sealed class Organisation : IDomainEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public double DefaultAoRadiusKm { get; set; } = 25.0;
    public int DefaultReputationScore { get; set; } = 50;
    public int MinimumReputationThreshold { get; set; } = 10;
    public double ExtendedNotificationRadiusKm { get; set; } = 50.0;
    public int KeeperInvitationExpiryDays { get; set; } = 7;
    public int KeeperRetentionDays { get; set; } = 365;
    public ReputationPointValues ReputationPointValues { get; set; } = ReputationPointValues.Defaults();
}
