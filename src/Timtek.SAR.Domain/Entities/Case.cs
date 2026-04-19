using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Domain.Entities;

public sealed class Case : IDomainEntity<Guid>
{
    static readonly Dictionary<CaseStatus, CaseStatus[]> ValidTransitions = new()
    {
        [CaseStatus.Reported] = [CaseStatus.Triaged],
        [CaseStatus.Triaged] = [CaseStatus.ActiveSearch, CaseStatus.Resolved],
        [CaseStatus.ActiveSearch] = [CaseStatus.Suspended, CaseStatus.Resolved],
        [CaseStatus.Suspended] = [CaseStatus.ActiveSearch, CaseStatus.Resolved],
        [CaseStatus.Resolved] = [],
    };

    public Guid Id { get; set; }
    public required Guid OrganisationId { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public string ReferenceNumber { get; set; } = GenerateReferenceNumber();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Animal description (FR-4.1.2)
    public required string AnimalSpecies { get; set; }
    public string? AnimalBreed { get; set; }
    public string? AnimalColour { get; set; }
    public string? AnimalSize { get; set; }
    public string? AnimalDistinguishingFeatures { get; set; }
    public string? AnimalPhotoUrl { get; set; }

    // Location & timing
    public required double LastKnownLatitude { get; set; }
    public required double LastKnownLongitude { get; set; }
    public required DateTime DateTimeLastSeen { get; set; }

    // Owner contact
    public required string OwnerName { get; set; }
    public string? OwnerPhone { get; set; }
    public required string OwnerEmail { get; set; }

    // Notes
    public string? MedicalNotes { get; set; }
    public string? BehaviouralNotes { get; set; }

    // Status workflow (FR-4.1.3)
    public CaseStatus Status { get; private set; } = CaseStatus.Reported;
    public CasePriority? Priority { get; private set; }
    public CaseOutcome? Outcome { get; private set; }

    // Activity log (FR-4.1.7)
    public List<CaseActivityLog> ActivityLog { get; set; } = [];

    public void Triage()
    {
        var oldStatus = Status;
        EnsureValidTransition(CaseStatus.Triaged);
        Status = CaseStatus.Triaged;
        RecordStatusChange(oldStatus, Status);
    }

    public void StartActiveSearch()
    {
        var oldStatus = Status;
        EnsureValidTransition(CaseStatus.ActiveSearch);
        Status = CaseStatus.ActiveSearch;
        RecordStatusChange(oldStatus, Status);
    }

    public void Suspend()
    {
        var oldStatus = Status;
        EnsureValidTransition(CaseStatus.Suspended);
        Status = CaseStatus.Suspended;
        RecordStatusChange(oldStatus, Status);
    }

    public void Resolve(CaseOutcome outcome)
    {
        var oldStatus = Status;
        EnsureValidTransition(CaseStatus.Resolved);
        Status = CaseStatus.Resolved;
        Outcome = outcome;
        RecordStatusChange(oldStatus, Status);
        ActivityLog.Add(new CaseActivityLog
        {
            CaseId = Id,
            UserId = CreatedByUserId,
            ActivityType = CaseActivityType.OutcomeAssigned,
            Description = $"Outcome set to {outcome}",
        });
    }

    public void SetPriority(CasePriority priority)
    {
        var oldPriority = Priority;
        Priority = priority;
        ActivityLog.Add(new CaseActivityLog
        {
            CaseId = Id,
            UserId = CreatedByUserId,
            ActivityType = CaseActivityType.PriorityChanged,
            Description = oldPriority is null
                ? $"Priority set to {priority}"
                : $"Priority changed from {oldPriority} to {priority}",
        });
    }

    public void AddNote(string note, Guid userId)
    {
        ActivityLog.Add(new CaseActivityLog
        {
            CaseId = Id,
            UserId = userId,
            ActivityType = CaseActivityType.NoteAdded,
            Description = note,
        });
    }

    void EnsureValidTransition(CaseStatus target)
    {
        if (!ValidTransitions[Status].Contains(target))
            throw new InvalidOperationException(
                $"Cannot transition from {Status} to {target}.");
    }

    void RecordStatusChange(CaseStatus oldStatus, CaseStatus newStatus)
    {
        ActivityLog.Add(new CaseActivityLog
        {
            CaseId = Id,
            UserId = CreatedByUserId,
            ActivityType = CaseActivityType.StatusChanged,
            Description = $"Status changed from {oldStatus} to {newStatus}",
        });
    }

    static string GenerateReferenceNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"SAR-{timestamp}-{suffix}";
    }
}
