using TA.Utils.Core;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Application.OrganisationManagement;

public interface IOrganisationService
{
    Task<Organisation> CreateAsync(string name);
    Task<Maybe<Organisation>> GetAsync(Guid id);
    Task<Maybe<Organisation>> GetFirstOrganisationAsync();
    Task UpdateSettingsAsync(Guid id, Action<Organisation> applyChanges);
}
