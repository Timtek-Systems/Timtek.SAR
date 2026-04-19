using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Infrastructure.Persistence;

[Subject("ApplicationUser Persistence")]
class When_round_tripping_an_application_user_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static ApplicationUser _saved;
    static ApplicationUser _loaded;
    static Guid _organisationId;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _organisationId = Guid.NewGuid();

        _saved = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "john.doe@example.com",
            NormalizedUserName = "JOHN.DOE@EXAMPLE.COM",
            Email = "john.doe@example.com",
            NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
            DisplayName = "John Doe",
            OrganisationId = _organisationId,
        };

        _writeContext.Users.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Users.Single(u => u.Id == _saved.Id);

    It should_persist_the_id = () => _loaded.Id.ShouldEqual(_saved.Id);
    It should_persist_the_username = () => _loaded.UserName.ShouldEqual("john.doe@example.com");
    It should_persist_the_email = () => _loaded.Email.ShouldEqual("john.doe@example.com");
    It should_persist_the_display_name = () => _loaded.DisplayName.ShouldEqual("John Doe");
    It should_persist_the_organisation_id = () => _loaded.OrganisationId.ShouldEqual(_organisationId);
    It should_be_active_by_default = () => _loaded.IsActive.ShouldBeTrue();

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("ApplicationUser Persistence")]
class When_persisting_a_deactivated_user
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static ApplicationUser _saved;
    static ApplicationUser _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "inactive@example.com",
            NormalizedUserName = "INACTIVE@EXAMPLE.COM",
            Email = "inactive@example.com",
            NormalizedEmail = "INACTIVE@EXAMPLE.COM",
            DisplayName = "Inactive User",
            OrganisationId = Guid.NewGuid(),
        };
        _saved.Deactivate();

        _writeContext.Users.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Users.Single(u => u.Id == _saved.Id);

    It should_persist_the_inactive_status = () => _loaded.IsActive.ShouldBeFalse();

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("ApplicationRole Persistence")]
class When_round_tripping_an_application_role_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static ApplicationRole _saved;
    static ApplicationRole _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new ApplicationRole("Administrator")
        {
            Id = Guid.NewGuid(),
            NormalizedName = "ADMINISTRATOR",
        };

        _writeContext.Roles.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Roles.Single(r => r.Id == _saved.Id);

    It should_persist_the_id = () => _loaded.Id.ShouldEqual(_saved.Id);
    It should_persist_the_name = () => _loaded.Name.ShouldEqual("Administrator");
    It should_persist_the_normalized_name = () => _loaded.NormalizedName.ShouldEqual("ADMINISTRATOR");

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}
