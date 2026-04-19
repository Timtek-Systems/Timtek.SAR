using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Application.OrganisationManagement;

public sealed class OrganisationService(
    IRepository<Organisation, Guid> repository,
    IUnitOfWork unitOfWork) : IOrganisationService
{
    public async Task<Organisation> CreateAsync(string name)
    {
        var organisation = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
        repository.Add(organisation);
        await unitOfWork.CommitAsync();
        return organisation;
    }

    public Task<Maybe<Organisation>> GetAsync(Guid id)
    {
        return Task.FromResult(repository.GetMaybe(id));
    }

    public Task<Maybe<Organisation>> GetFirstOrganisationAsync()
    {
        var first = repository.GetAll().FirstOrDefault();
        return Task.FromResult(first is not null ? first.AsMaybe() : Maybe<Organisation>.Empty);
    }

    public async Task UpdateSettingsAsync(Guid id, Action<Organisation> applyChanges)
    {
        var organisation = repository.GetMaybe(id).Single();
        applyChanges(organisation);
        await unitOfWork.CommitAsync();
    }
}
