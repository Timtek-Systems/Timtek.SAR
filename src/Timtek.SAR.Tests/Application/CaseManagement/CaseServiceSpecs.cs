using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.CaseManagement;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Application.CaseManagement;

class CaseServiceContextBuilder
{
    readonly IRepository<Case, Guid> _caseRepository = A.Fake<IRepository<Case, Guid>>();
    readonly IRepository<CaseActivityLog, Guid> _activityLogRepository = A.Fake<IRepository<CaseActivityLog, Guid>>();
    readonly IUnitOfWork _unitOfWork = A.Fake<IUnitOfWork>();
    readonly List<Case> _existingCases = [];
    readonly List<CaseActivityLog> _existingLogs = [];

    public CaseServiceContextBuilder WithExistingCase(Case sarCase)
    {
        _existingCases.Add(sarCase);
        A.CallTo(() => _caseRepository.GetMaybe(sarCase.Id)).Returns(sarCase.AsMaybe());
        A.CallTo(() => _caseRepository.GetAll()).Returns(_existingCases);
        return this;
    }

    public CaseServiceContextBuilder WithNoCase(Guid id)
    {
        A.CallTo(() => _caseRepository.GetMaybe(id)).Returns(Maybe<Case>.Empty);
        return this;
    }

    public CaseServiceContextBuilder WithActivityLogs(params CaseActivityLog[] logs)
    {
        _existingLogs.AddRange(logs);
        A.CallTo(() => _activityLogRepository.GetAll()).Returns(_existingLogs);
        return this;
    }

    public ICaseService Build() => new CaseService(_caseRepository, _activityLogRepository, _unitOfWork);

    public IRepository<Case, Guid> CaseRepository => _caseRepository;
    public IUnitOfWork UnitOfWork => _unitOfWork;
}

static class CaseServiceTestHelper
{
    public static CreateCaseRequest CreateRequest(Guid? orgId = null, Guid? userId = null) => new(
        OrganisationId: orgId ?? Guid.NewGuid(),
        CreatedByUserId: userId ?? Guid.NewGuid(),
        AnimalSpecies: "Dog",
        AnimalBreed: "Labrador",
        AnimalColour: "Golden",
        AnimalSize: "Large",
        AnimalDistinguishingFeatures: null,
        AnimalPhotoUrl: null,
        LastKnownLatitude: 50.7184,
        LastKnownLongitude: -3.5339,
        DateTimeLastSeen: DateTime.UtcNow,
        OwnerName: "Jane Smith",
        OwnerPhone: "07700900123",
        OwnerEmail: "jane@example.com",
        MedicalNotes: null,
        BehaviouralNotes: null);

    public static Case CreateExistingCase(Guid? id = null, Guid? orgId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        OrganisationId = orgId ?? Guid.NewGuid(),
        CreatedByUserId = Guid.NewGuid(),
        AnimalSpecies = "Cat",
        LastKnownLatitude = 51.5,
        LastKnownLongitude = -0.1,
        DateTimeLastSeen = DateTime.UtcNow,
        OwnerName = "Test Owner",
        OwnerEmail = "test@example.com",
    };
}

// --- CreateAsync ---

[Subject("Case Service")]
class When_creating_a_case
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static CaseDto _result;

    Establish context = () =>
    {
        _builder = new CaseServiceContextBuilder();
        _service = _builder.Build();
    };

    Because of = () => _result = _service.CreateAsync(CaseServiceTestHelper.CreateRequest()).Await();

    It should_return_a_case_dto = () => _result.ShouldNotBeNull();
    It should_have_a_generated_id = () => _result.Id.ShouldNotEqual(Guid.Empty);
    It should_have_reported_status = () => _result.Status.ShouldEqual(CaseStatus.Reported);
    It should_have_a_reference_number = () => _result.ReferenceNumber.ShouldNotBeEmpty();
    It should_set_the_species = () => _result.AnimalSpecies.ShouldEqual("Dog");
    It should_add_to_repository = () =>
        A.CallTo(() => _builder.CaseRepository.Add(A<Case>.Ignored)).MustHaveHappenedOnceExactly();
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

// --- GetAsync ---

[Subject("Case Service")]
class When_getting_an_existing_case
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;
    static Maybe<CaseDto> _result;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _result = _service.GetAsync(_existing.Id).Await();

    It should_return_the_case = () => _result.Any().ShouldBeTrue();
    It should_have_correct_species = () => _result.Single().AnimalSpecies.ShouldEqual("Cat");
}

[Subject("Case Service")]
class When_getting_a_nonexistent_case
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Guid _missingId;
    static Maybe<CaseDto> _result;

    Establish context = () =>
    {
        _missingId = Guid.NewGuid();
        _builder = new CaseServiceContextBuilder().WithNoCase(_missingId);
        _service = _builder.Build();
    };

    Because of = () => _result = _service.GetAsync(_missingId).Await();

    It should_return_empty = () => _result.Any().ShouldBeFalse();
}

// --- GetByOrganisationAsync ---

[Subject("Case Service")]
class When_getting_cases_by_organisation
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Guid _orgId;
    static IReadOnlyList<CaseDto> _result;

    Establish context = () =>
    {
        _orgId = Guid.NewGuid();
        var case1 = CaseServiceTestHelper.CreateExistingCase(orgId: _orgId);
        var case2 = CaseServiceTestHelper.CreateExistingCase(orgId: _orgId);
        var otherCase = CaseServiceTestHelper.CreateExistingCase(orgId: Guid.NewGuid());
        _builder = new CaseServiceContextBuilder()
            .WithExistingCase(case1)
            .WithExistingCase(case2)
            .WithExistingCase(otherCase);
        _service = _builder.Build();
    };

    Because of = () => _result = _service.GetByOrganisationAsync(_orgId).GetAwaiter().GetResult();

    It should_return_only_matching_cases = () => _result.Count.ShouldEqual(2);
}

// --- Status transitions ---

[Subject("Case Service")]
class When_triaging_a_case_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _service.TriageAsync(_existing.Id).Await();

    It should_change_status_to_triaged = () => _existing.Status.ShouldEqual(CaseStatus.Triaged);
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("Case Service")]
class When_starting_active_search_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _existing.Triage();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _service.StartActiveSearchAsync(_existing.Id).Await();

    It should_change_status = () => _existing.Status.ShouldEqual(CaseStatus.ActiveSearch);
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("Case Service")]
class When_resolving_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _existing.Triage();
        _existing.StartActiveSearch();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _service.ResolveAsync(_existing.Id, CaseOutcome.Recovered).Await();

    It should_change_status = () => _existing.Status.ShouldEqual(CaseStatus.Resolved);
    It should_set_outcome = () => _existing.Outcome.ShouldEqual(CaseOutcome.Recovered);
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

// --- Priority ---

[Subject("Case Service")]
class When_setting_priority_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _service.SetPriorityAsync(_existing.Id, CasePriority.Critical).Await();

    It should_set_the_priority = () => _existing.Priority.ShouldEqual(CasePriority.Critical);
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

// --- Notes ---

[Subject("Case Service")]
class When_adding_a_note_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Case _existing;
    static Guid _userId;

    Establish context = () =>
    {
        _existing = CaseServiceTestHelper.CreateExistingCase();
        _userId = Guid.NewGuid();
        _builder = new CaseServiceContextBuilder().WithExistingCase(_existing);
        _service = _builder.Build();
    };

    Because of = () => _service.AddNoteAsync(_existing.Id, _userId, "Possible sighting near river").Await();

    It should_add_note_to_activity_log = () => _existing.ActivityLog.ShouldContain(e => e.ActivityType == CaseActivityType.NoteAdded);
    It should_commit = () =>
        A.CallTo(() => _builder.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

// --- Activity log retrieval ---

[Subject("Case Service")]
class When_getting_activity_log_through_service
{
    static CaseServiceContextBuilder _builder;
    static ICaseService _service;
    static Guid _caseId;
    static IReadOnlyList<CaseActivityLogDto> _result;

    Establish context = () =>
    {
        _caseId = Guid.NewGuid();
        var log1 = new CaseActivityLog
        {
            Id = Guid.NewGuid(),
            CaseId = _caseId,
            UserId = Guid.NewGuid(),
            ActivityType = CaseActivityType.StatusChanged,
            Description = "Status changed",
            TimestampUtc = DateTime.UtcNow.AddMinutes(-5),
        };
        var log2 = new CaseActivityLog
        {
            Id = Guid.NewGuid(),
            CaseId = _caseId,
            UserId = Guid.NewGuid(),
            ActivityType = CaseActivityType.NoteAdded,
            Description = "Note added",
            TimestampUtc = DateTime.UtcNow,
        };
        var otherLog = new CaseActivityLog
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ActivityType = CaseActivityType.Created,
            Description = "Other case",
            TimestampUtc = DateTime.UtcNow,
        };
        _builder = new CaseServiceContextBuilder().WithActivityLogs(log1, log2, otherLog);
        _service = _builder.Build();
    };

    Because of = () => _result = _service.GetActivityLogAsync(_caseId).GetAwaiter().GetResult();

    It should_return_only_logs_for_the_case = () => _result.Count.ShouldEqual(2);
    It should_return_newest_first = () => _result[0].ActivityType.ShouldEqual(CaseActivityType.NoteAdded);
}
