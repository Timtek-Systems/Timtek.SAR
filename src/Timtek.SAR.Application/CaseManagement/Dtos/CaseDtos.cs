using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.CaseManagement.Dtos;

public sealed record CreateCaseRequest(
    Guid OrganisationId,
    Guid CreatedByUserId,
    string AnimalSpecies,
    string? AnimalBreed,
    string? AnimalColour,
    string? AnimalSize,
    string? AnimalDistinguishingFeatures,
    string? AnimalPhotoUrl,
    double LastKnownLatitude,
    double LastKnownLongitude,
    DateTime DateTimeLastSeen,
    string OwnerName,
    string? OwnerPhone,
    string OwnerEmail,
    string? MedicalNotes,
    string? BehaviouralNotes);

public sealed record CaseDto(
    Guid Id,
    string ReferenceNumber,
    Guid OrganisationId,
    Guid CreatedByUserId,
    DateTime CreatedAtUtc,
    string AnimalSpecies,
    string? AnimalBreed,
    string? AnimalColour,
    string? AnimalSize,
    string? AnimalDistinguishingFeatures,
    string? AnimalPhotoUrl,
    double LastKnownLatitude,
    double LastKnownLongitude,
    DateTime DateTimeLastSeen,
    string OwnerName,
    string? OwnerPhone,
    string OwnerEmail,
    string? MedicalNotes,
    string? BehaviouralNotes,
    CaseStatus Status,
    CasePriority? Priority,
    CaseOutcome? Outcome);

public sealed record CaseActivityLogDto(
    Guid Id,
    Guid CaseId,
    Guid UserId,
    CaseActivityType ActivityType,
    string Description,
    DateTime TimestampUtc);
