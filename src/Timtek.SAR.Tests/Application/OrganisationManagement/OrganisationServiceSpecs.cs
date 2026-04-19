using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.OrganisationManagement;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Tests.Application.OrganisationManagement;

class OrganisationServiceContextBuilder
{
    readonly IRepository<Organisation, Guid> _repository = A.Fake<IRepository<Organisation, Guid>>();
    readonly IUnitOfWork _unitOfWork = A.Fake<IUnitOfWork>();
    Organisation? _existingOrganisation;

    public OrganisationServiceContextBuilder WithExistingOrganisation(Organisation org)
    {
        _existingOrganisation = org;
        A.CallTo(() => _repository.GetMaybe(org.Id)).Returns(org.AsMaybe());
        A.CallTo(() => _repository.GetAll()).Returns(new[] { org });
        return this;
    }

    public OrganisationServiceContextBuilder WithNoOrganisation(Guid id)
    {
        A.CallTo(() => _repository.GetMaybe(id)).Returns(Maybe<Organisation>.Empty);
        return this;
    }

    public OrganisationServiceContextBuilder WithNoOrganisations()
    {
        A.CallTo(() => _repository.GetAll()).Returns(Enumerable.Empty<Organisation>());
        return this;
    }

    public IOrganisationService Build() => new OrganisationService(_repository, _unitOfWork);

    public IRepository<Organisation, Guid> Repository => _repository;
    public IUnitOfWork UnitOfWork => _unitOfWork;
}

[Subject("Organisation Service")]
class When_creating_a_new_organisation
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Organisation _result;

    Establish context = () =>
    {
        _context = new OrganisationServiceContextBuilder();
        _service = _context.Build();
    };

    Because of = () => _result = _service.CreateAsync("Devon SAR").Await();

    It should_return_the_created_organisation = () => _result.ShouldNotBeNull();
    It should_set_the_name = () => _result.Name.ShouldEqual("Devon SAR");
    It should_assign_an_id = () => _result.Id.ShouldNotEqual(Guid.Empty);
    It should_add_it_to_the_repository = () =>
        A.CallTo(() => _context.Repository.Add(A<Organisation>.That.Matches(o => o.Name == "Devon SAR")))
            .MustHaveHappenedOnceExactly();
    It should_commit_the_unit_of_work = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
    It should_have_default_settings = () => _result.DefaultAoRadiusKm.ShouldEqual(25.0);
}

[Subject("Organisation Service")]
class When_getting_an_existing_organisation
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Organisation _existing;
    static Maybe<Organisation> _result;

    Establish context = () =>
    {
        _existing = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };
        _context = new OrganisationServiceContextBuilder().WithExistingOrganisation(_existing);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetAsync(_existing.Id).Await();

    It should_return_the_organisation = () => _result.Any().ShouldBeTrue();
    It should_have_the_correct_name = () => _result.Single().Name.ShouldEqual("Devon SAR");
}

[Subject("Organisation Service")]
class When_getting_a_nonexistent_organisation
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Guid _missingId;
    static Maybe<Organisation> _result;

    Establish context = () =>
    {
        _missingId = Guid.NewGuid();
        _context = new OrganisationServiceContextBuilder().WithNoOrganisation(_missingId);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetAsync(_missingId).Await();

    It should_return_empty = () => _result.Any().ShouldBeFalse();
}

[Subject("Organisation Service")]
class When_updating_organisation_settings
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Organisation _existing;

    Establish context = () =>
    {
        _existing = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };
        _context = new OrganisationServiceContextBuilder().WithExistingOrganisation(_existing);
        _service = _context.Build();
    };

    Because of = () => _service.UpdateSettingsAsync(_existing.Id, org =>
    {
        org.DefaultAoRadiusKm = 30.0;
        org.MinimumReputationThreshold = 20;
    }).Await();

    It should_update_the_ao_radius = () => _existing.DefaultAoRadiusKm.ShouldEqual(30.0);
    It should_update_the_threshold = () => _existing.MinimumReputationThreshold.ShouldEqual(20);
    It should_commit_the_unit_of_work = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("Organisation Service")]
class When_getting_the_first_organisation_when_one_exists
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Organisation _existing;
    static Maybe<Organisation> _result;

    Establish context = () =>
    {
        _existing = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };
        _context = new OrganisationServiceContextBuilder().WithExistingOrganisation(_existing);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetFirstOrganisationAsync().Await();

    It should_return_an_organisation = () => _result.Any().ShouldBeTrue();
    It should_return_the_correct_organisation = () => _result.Single().Name.ShouldEqual("Devon SAR");
}

[Subject("Organisation Service")]
class When_getting_the_first_organisation_when_none_exist
{
    static OrganisationServiceContextBuilder _context;
    static IOrganisationService _service;
    static Maybe<Organisation> _result;

    Establish context = () =>
    {
        _context = new OrganisationServiceContextBuilder().WithNoOrganisations();
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetFirstOrganisationAsync().Await();

    It should_return_empty = () => _result.Any().ShouldBeFalse();
}
