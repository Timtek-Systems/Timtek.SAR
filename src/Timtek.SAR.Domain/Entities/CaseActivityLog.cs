using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Domain.Entities;

public sealed class CaseActivityLog : IDomainEntity<Guid>
{
    public Guid Id { get; set; }
    public required Guid CaseId { get; set; }
    public required Guid UserId { get; set; }
    public required CaseActivityType ActivityType { get; set; }
    public required string Description { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
