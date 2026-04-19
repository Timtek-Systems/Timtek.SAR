namespace Timtek.SAR.Domain.ValueObjects;

public sealed class ReputationPointValues
{
    public int JoinCase { get; set; }
    public int FoundOutcome { get; set; }
    public int ConfirmedSighting { get; set; }
    public int EvidenceUpload { get; set; }
    public int EvidenceUploadCapPerCase { get; set; }
    public int TrackUpload { get; set; }
    public int SectorCompletion { get; set; }
    public int FirstCaseBonus { get; set; }
    public int MissedInScopeCase { get; set; }
    public int RemovedFromCase { get; set; }
    public int DismissedSighting { get; set; }

    public static ReputationPointValues Defaults() => new()
    {
        JoinCase = 5,
        FoundOutcome = 20,
        ConfirmedSighting = 10,
        EvidenceUpload = 2,
        EvidenceUploadCapPerCase = 10,
        TrackUpload = 3,
        SectorCompletion = 5,
        FirstCaseBonus = 10,
        MissedInScopeCase = -10,
        RemovedFromCase = -15,
        DismissedSighting = -3,
    };
}
