namespace Timtek.SAR.Application.OrganisationManagement.Dtos;

public sealed record CreateOrganisationRequest(string Name);

public sealed record UpdateOrganisationSettingsRequest(
    double DefaultAoRadiusKm,
    int DefaultReputationScore,
    int MinimumReputationThreshold,
    double ExtendedNotificationRadiusKm,
    int KeeperInvitationExpiryDays,
    int KeeperRetentionDays);

public sealed record OrganisationResponse(
    Guid Id,
    string Name,
    double DefaultAoRadiusKm,
    int DefaultReputationScore,
    int MinimumReputationThreshold,
    double ExtendedNotificationRadiusKm,
    int KeeperInvitationExpiryDays,
    int KeeperRetentionDays,
    ReputationPointValuesResponse ReputationPointValues);

public sealed record ReputationPointValuesResponse(
    int JoinCase,
    int FoundOutcome,
    int ConfirmedSighting,
    int EvidenceUpload,
    int EvidenceUploadCapPerCase,
    int TrackUpload,
    int SectorCompletion,
    int FirstCaseBonus,
    int MissedInScopeCase,
    int RemovedFromCase,
    int DismissedSighting);
