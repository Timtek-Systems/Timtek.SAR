using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Infrastructure.Persistence;

[Subject("Case Persistence")]
class When_round_tripping_a_case_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Case _saved;
    static Case _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Case
        {
            Id = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            CreatedByUserId = Guid.NewGuid(),
            AnimalSpecies = "Dog",
            AnimalBreed = "Labrador",
            AnimalColour = "Golden",
            AnimalSize = "Large",
            AnimalDistinguishingFeatures = "Red collar with name tag",
            AnimalPhotoUrl = "https://example.com/lab.jpg",
            LastKnownLatitude = 50.7184,
            LastKnownLongitude = -3.5339,
            DateTimeLastSeen = new DateTime(2026, 4, 19, 14, 0, 0, DateTimeKind.Utc),
            OwnerName = "John Doe",
            OwnerPhone = "07700900456",
            OwnerEmail = "john@example.com",
            MedicalNotes = "Allergic to chicken",
            BehaviouralNotes = "Friendly but skittish",
        };

        _writeContext.Cases.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Cases.AsNoTracking().Single(c => c.Id == _saved.Id);

    It should_persist_the_reference_number = () => _loaded.ReferenceNumber.ShouldEqual(_saved.ReferenceNumber);
    It should_persist_the_organisation = () => _loaded.OrganisationId.ShouldEqual(_saved.OrganisationId);
    It should_persist_the_creator = () => _loaded.CreatedByUserId.ShouldEqual(_saved.CreatedByUserId);
    It should_persist_the_species = () => _loaded.AnimalSpecies.ShouldEqual("Dog");
    It should_persist_the_breed = () => _loaded.AnimalBreed.ShouldEqual("Labrador");
    It should_persist_the_colour = () => _loaded.AnimalColour.ShouldEqual("Golden");
    It should_persist_the_size = () => _loaded.AnimalSize.ShouldEqual("Large");
    It should_persist_the_distinguishing_features = () => _loaded.AnimalDistinguishingFeatures.ShouldEqual("Red collar with name tag");
    It should_persist_the_photo_url = () => _loaded.AnimalPhotoUrl.ShouldEqual("https://example.com/lab.jpg");
    It should_persist_the_latitude = () => _loaded.LastKnownLatitude.ShouldEqual(50.7184);
    It should_persist_the_longitude = () => _loaded.LastKnownLongitude.ShouldEqual(-3.5339);
    It should_persist_the_date_time_last_seen = () => _loaded.DateTimeLastSeen.ShouldEqual(new DateTime(2026, 4, 19, 14, 0, 0, DateTimeKind.Utc));
    It should_persist_the_owner_name = () => _loaded.OwnerName.ShouldEqual("John Doe");
    It should_persist_the_owner_phone = () => _loaded.OwnerPhone.ShouldEqual("07700900456");
    It should_persist_the_owner_email = () => _loaded.OwnerEmail.ShouldEqual("john@example.com");
    It should_persist_the_medical_notes = () => _loaded.MedicalNotes.ShouldEqual("Allergic to chicken");
    It should_persist_the_behavioural_notes = () => _loaded.BehaviouralNotes.ShouldEqual("Friendly but skittish");
    It should_persist_the_status_as_reported = () => _loaded.Status.ShouldEqual(CaseStatus.Reported);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Case Persistence")]
class When_round_tripping_a_case_with_activity_log
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Case _saved;
    static Case _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Case
        {
            Id = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            CreatedByUserId = Guid.NewGuid(),
            AnimalSpecies = "Cat",
            LastKnownLatitude = 51.5,
            LastKnownLongitude = -0.1,
            DateTimeLastSeen = DateTime.UtcNow,
            OwnerName = "Jane",
            OwnerEmail = "jane@example.com",
        };
        _saved.Triage();
        _saved.SetPriority(CasePriority.High);

        _writeContext.Cases.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Cases
        .AsNoTracking()
        .Include(c => c.ActivityLog)
        .Single(c => c.Id == _saved.Id);

    It should_persist_the_triaged_status = () => _loaded.Status.ShouldEqual(CaseStatus.Triaged);
    It should_persist_the_priority = () => _loaded.Priority.ShouldEqual(CasePriority.High);
    It should_persist_the_activity_log_entries = () => _loaded.ActivityLog.Count.ShouldEqual(2);
    It should_persist_a_status_change_entry = () => _loaded.ActivityLog.ShouldContain(e => e.ActivityType == CaseActivityType.StatusChanged);
    It should_persist_a_priority_change_entry = () => _loaded.ActivityLog.ShouldContain(e => e.ActivityType == CaseActivityType.PriorityChanged);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Case Persistence")]
class When_round_tripping_a_resolved_case
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Case _saved;
    static Case _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Case
        {
            Id = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            CreatedByUserId = Guid.NewGuid(),
            AnimalSpecies = "Parrot",
            LastKnownLatitude = 52.0,
            LastKnownLongitude = -1.0,
            DateTimeLastSeen = DateTime.UtcNow,
            OwnerName = "Bob",
            OwnerEmail = "bob@example.com",
        };
        _saved.Triage();
        _saved.StartActiveSearch();
        _saved.Resolve(CaseOutcome.Reunited);

        _writeContext.Cases.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Cases.AsNoTracking().Single(c => c.Id == _saved.Id);

    It should_persist_resolved_status = () => _loaded.Status.ShouldEqual(CaseStatus.Resolved);
    It should_persist_the_outcome = () => _loaded.Outcome.ShouldEqual(CaseOutcome.Reunited);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Case Persistence")]
class When_querying_cases_by_organisation
{
    static SarDbContext _context;
    static SqliteConnection _connection;
    static Guid _orgId;
    static List<Case> _results;

    Establish context_setup = () =>
    {
        (_context, _connection) = SarDbContextFactory.CreateInMemory();
        _orgId = Guid.NewGuid();
        var otherOrgId = Guid.NewGuid();

        _context.Cases.AddRange(
            CreateCase(_orgId, "Dog"),
            CreateCase(_orgId, "Cat"),
            CreateCase(otherOrgId, "Hamster")
        );
        _context.SaveChanges();
    };

    Because of = () => _results = _context.Cases
        .AsNoTracking()
        .Where(c => c.OrganisationId == _orgId)
        .ToList();

    It should_return_only_cases_for_the_organisation = () => _results.Count.ShouldEqual(2);
    It should_not_include_other_organisations = () => _results.ShouldEachConformTo(c => c.OrganisationId == _orgId);

    Cleanup after = () =>
    {
        _context?.Dispose();
        _connection?.Dispose();
    };

    static Case CreateCase(Guid orgId, string species) => new()
    {
        Id = Guid.NewGuid(),
        OrganisationId = orgId,
        CreatedByUserId = Guid.NewGuid(),
        AnimalSpecies = species,
        LastKnownLatitude = 51.5,
        LastKnownLongitude = -0.1,
        DateTimeLastSeen = DateTime.UtcNow,
        OwnerName = "Owner",
        OwnerEmail = "owner@example.com",
    };
}

[Subject("Case Persistence")]
class When_triaging_a_case_loaded_via_find_in_a_separate_context
{
    static SarDbContext _writeContext;
    static SarDbContext _updateContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Case _saved;
    static Case _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Case
        {
            Id = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            CreatedByUserId = Guid.NewGuid(),
            AnimalSpecies = "Dog",
            LastKnownLatitude = 51.5,
            LastKnownLongitude = -0.1,
            DateTimeLastSeen = DateTime.UtcNow,
            OwnerName = "Owner",
            OwnerEmail = "owner@example.com",
        };
        _writeContext.Cases.Add(_saved);
        _writeContext.SaveChanges();

        var updateOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _updateContext = new SarDbContext(updateOptions);
    };

    Because of = () =>
    {
        var entity = _updateContext.Cases.Find(_saved.Id)!;
        entity.Triage();
        _updateContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
        _loaded = _readContext.Cases.Include(c => c.ActivityLog).AsNoTracking().Single(c => c.Id == _saved.Id);
    };

    It should_have_triaged_status = () => _loaded.Status.ShouldEqual(CaseStatus.Triaged);
    It should_have_activity_log_entry = () => _loaded.ActivityLog.Count.ShouldEqual(1);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _updateContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}
