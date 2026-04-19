using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.CaseManagement;

public sealed class CaseService(
    IRepository<Case, Guid> caseRepository,
    IRepository<CaseActivityLog, Guid> activityLogRepository,
    IUnitOfWork unitOfWork) : ICaseService
{
    public async Task<CaseDto> CreateAsync(CreateCaseRequest request)
    {
        var sarCase = new Case
        {
            Id = Guid.NewGuid(),
            OrganisationId = request.OrganisationId,
            CreatedByUserId = request.CreatedByUserId,
            AnimalSpecies = request.AnimalSpecies,
            AnimalBreed = request.AnimalBreed,
            AnimalColour = request.AnimalColour,
            AnimalSize = request.AnimalSize,
            AnimalDistinguishingFeatures = request.AnimalDistinguishingFeatures,
            AnimalPhotoUrl = request.AnimalPhotoUrl,
            LastKnownLatitude = request.LastKnownLatitude,
            LastKnownLongitude = request.LastKnownLongitude,
            DateTimeLastSeen = request.DateTimeLastSeen,
            OwnerName = request.OwnerName,
            OwnerPhone = request.OwnerPhone,
            OwnerEmail = request.OwnerEmail,
            MedicalNotes = request.MedicalNotes,
            BehaviouralNotes = request.BehaviouralNotes,
        };
        caseRepository.Add(sarCase);
        await unitOfWork.CommitAsync();
        return ToDto(sarCase);
    }

    public Task<Maybe<CaseDto>> GetAsync(Guid id)
    {
        var result = caseRepository.GetMaybe(id);
        if (!result.Any())
            return Task.FromResult(Maybe<CaseDto>.Empty);
        return Task.FromResult(ToDto(result.Single()).AsMaybe());
    }

    public Task<IReadOnlyList<CaseDto>> GetByOrganisationAsync(Guid organisationId)
    {
        var cases = caseRepository.GetAll()
            .Where(c => c.OrganisationId == organisationId)
            .Select(c => ToDto(c))
            .ToList();
        return Task.FromResult<IReadOnlyList<CaseDto>>(cases);
    }

    public async Task TriageAsync(Guid caseId)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.Triage();
        await unitOfWork.CommitAsync();
    }

    public async Task StartActiveSearchAsync(Guid caseId)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.StartActiveSearch();
        await unitOfWork.CommitAsync();
    }

    public async Task SuspendAsync(Guid caseId)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.Suspend();
        await unitOfWork.CommitAsync();
    }

    public async Task ResolveAsync(Guid caseId, CaseOutcome outcome)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.Resolve(outcome);
        await unitOfWork.CommitAsync();
    }

    public async Task SetPriorityAsync(Guid caseId, CasePriority priority)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.SetPriority(priority);
        await unitOfWork.CommitAsync();
    }

    public async Task AddNoteAsync(Guid caseId, Guid userId, string note)
    {
        var sarCase = caseRepository.GetMaybe(caseId).Single();
        sarCase.AddNote(note, userId);
        await unitOfWork.CommitAsync();
    }

    public Task<IReadOnlyList<CaseActivityLogDto>> GetActivityLogAsync(Guid caseId)
    {
        var logs = activityLogRepository.GetAll()
            .Where(a => a.CaseId == caseId)
            .OrderByDescending(a => a.TimestampUtc)
            .Select(a => new CaseActivityLogDto(
                a.Id, a.CaseId, a.UserId, a.ActivityType, a.Description, a.TimestampUtc))
            .ToList();
        return Task.FromResult<IReadOnlyList<CaseActivityLogDto>>(logs);
    }

    static CaseDto ToDto(Case c) => new(
        c.Id, c.ReferenceNumber, c.OrganisationId, c.CreatedByUserId, c.CreatedAtUtc,
        c.AnimalSpecies, c.AnimalBreed, c.AnimalColour, c.AnimalSize,
        c.AnimalDistinguishingFeatures, c.AnimalPhotoUrl,
        c.LastKnownLatitude, c.LastKnownLongitude, c.DateTimeLastSeen,
        c.OwnerName, c.OwnerPhone, c.OwnerEmail,
        c.MedicalNotes, c.BehaviouralNotes,
        c.Status, c.Priority, c.Outcome);
}
