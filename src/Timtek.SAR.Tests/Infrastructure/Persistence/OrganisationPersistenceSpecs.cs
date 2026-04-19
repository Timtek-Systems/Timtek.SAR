using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.ValueObjects;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Infrastructure.Persistence;

static class SarDbContextFactory
{
    public static (SarDbContext context, SqliteConnection connection) CreateInMemory()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new SarDbContext(options);
        context.Database.EnsureCreated();
        return (context, connection);
    }
}

[Subject("Organisation Persistence")]
class When_round_tripping_an_organisation_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Organisation _saved;
    static Organisation _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = "Devon SAR",
            DefaultAoRadiusKm = 30.0,
            DefaultReputationScore = 60,
            MinimumReputationThreshold = 15,
            ExtendedNotificationRadiusKm = 75.0,
            KeeperInvitationExpiryDays = 14,
            KeeperRetentionDays = 180,
            ReputationPointValues = new ReputationPointValues
            {
                JoinCase = 10,
                FoundOutcome = 25,
                ConfirmedSighting = 15,
                EvidenceUpload = 3,
                EvidenceUploadCapPerCase = 12,
                TrackUpload = 4,
                SectorCompletion = 6,
                FirstCaseBonus = 12,
                MissedInScopeCase = -8,
                RemovedFromCase = -12,
                DismissedSighting = -2,
            },
        };

        _writeContext.Organisations.Add(_saved);
        _writeContext.SaveChanges();

        // Read back from a fresh context to verify persistence (not just cache)
        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Organisations.Single(o => o.Id == _saved.Id);

    It should_persist_the_id = () => _loaded.Id.ShouldEqual(_saved.Id);
    It should_persist_the_name = () => _loaded.Name.ShouldEqual("Devon SAR");
    It should_persist_the_default_ao_radius = () => _loaded.DefaultAoRadiusKm.ShouldEqual(30.0);
    It should_persist_the_default_reputation_score = () => _loaded.DefaultReputationScore.ShouldEqual(60);
    It should_persist_the_minimum_reputation_threshold = () => _loaded.MinimumReputationThreshold.ShouldEqual(15);
    It should_persist_the_extended_notification_radius = () => _loaded.ExtendedNotificationRadiusKm.ShouldEqual(75.0);
    It should_persist_the_keeper_invitation_expiry = () => _loaded.KeeperInvitationExpiryDays.ShouldEqual(14);
    It should_persist_the_keeper_retention_days = () => _loaded.KeeperRetentionDays.ShouldEqual(180);

    It should_persist_reputation_join_case = () => _loaded.ReputationPointValues.JoinCase.ShouldEqual(10);
    It should_persist_reputation_found_outcome = () => _loaded.ReputationPointValues.FoundOutcome.ShouldEqual(25);
    It should_persist_reputation_confirmed_sighting = () => _loaded.ReputationPointValues.ConfirmedSighting.ShouldEqual(15);
    It should_persist_reputation_evidence_upload = () => _loaded.ReputationPointValues.EvidenceUpload.ShouldEqual(3);
    It should_persist_reputation_evidence_cap = () => _loaded.ReputationPointValues.EvidenceUploadCapPerCase.ShouldEqual(12);
    It should_persist_reputation_track_upload = () => _loaded.ReputationPointValues.TrackUpload.ShouldEqual(4);
    It should_persist_reputation_sector_completion = () => _loaded.ReputationPointValues.SectorCompletion.ShouldEqual(6);
    It should_persist_reputation_first_case_bonus = () => _loaded.ReputationPointValues.FirstCaseBonus.ShouldEqual(12);
    It should_persist_reputation_missed_case = () => _loaded.ReputationPointValues.MissedInScopeCase.ShouldEqual(-8);
    It should_persist_reputation_removed_from_case = () => _loaded.ReputationPointValues.RemovedFromCase.ShouldEqual(-12);
    It should_persist_reputation_dismissed_sighting = () => _loaded.ReputationPointValues.DismissedSighting.ShouldEqual(-2);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Organisation Persistence")]
class When_round_tripping_an_organisation_with_default_values
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Organisation _saved;
    static Organisation _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = "Cornwall SAR",
        };

        _writeContext.Organisations.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Organisations.Single(o => o.Id == _saved.Id);

    It should_have_default_ao_radius = () => _loaded.DefaultAoRadiusKm.ShouldEqual(25.0);
    It should_have_default_reputation_score = () => _loaded.DefaultReputationScore.ShouldEqual(50);
    It should_have_default_minimum_threshold = () => _loaded.MinimumReputationThreshold.ShouldEqual(10);
    It should_have_default_reputation_join_case = () => _loaded.ReputationPointValues.JoinCase.ShouldEqual(5);
    It should_have_default_reputation_found_outcome = () => _loaded.ReputationPointValues.FoundOutcome.ShouldEqual(20);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}
