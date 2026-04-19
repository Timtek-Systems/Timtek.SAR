using TA.Utils.Core;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Application.CaseManagement;

public interface ICaseService
{
    Task<CaseDto> CreateAsync(CreateCaseRequest request);
    Task<Maybe<CaseDto>> GetAsync(Guid id);
    Task<IReadOnlyList<CaseDto>> GetByOrganisationAsync(Guid organisationId);
    Task TriageAsync(Guid caseId);
    Task StartActiveSearchAsync(Guid caseId);
    Task SuspendAsync(Guid caseId);
    Task ResolveAsync(Guid caseId, CaseOutcome outcome);
    Task SetPriorityAsync(Guid caseId, CasePriority priority);
    Task AddNoteAsync(Guid caseId, Guid userId, string note);
    Task<IReadOnlyList<CaseActivityLogDto>> GetActivityLogAsync(Guid caseId);
}
