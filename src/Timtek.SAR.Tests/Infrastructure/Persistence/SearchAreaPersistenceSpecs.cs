using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Infrastructure.Persistence;

[Subject("SearchArea Persistence")]
class When_round_tripping_a_search_area_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static SearchArea _saved;
    static SearchArea _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new SearchArea
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Name = "River Bank Zone",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };

        _writeContext.SearchAreas.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.SearchAreas.AsNoTracking().Single(a => a.Id == _saved.Id);

    It should_persist_the_name = () => _loaded.Name.ShouldEqual("River Bank Zone");
    It should_persist_the_case_id = () => _loaded.CaseId.ShouldEqual(_saved.CaseId);
    It should_persist_the_area_type = () => _loaded.AreaType.ShouldEqual(SearchAreaType.Polygon);
    It should_persist_the_geojson = () => _loaded.GeoJson.ShouldEqual(_saved.GeoJson);
    It should_persist_the_creation_timestamp = () => _loaded.CreatedAtUtc.ShouldBeCloseTo(_saved.CreatedAtUtc, TimeSpan.FromSeconds(1));

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("SearchArea Persistence")]
class When_round_tripping_a_circle_search_area
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static SearchArea _saved;
    static SearchArea _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new SearchArea
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Name = "Last Seen Radius",
            AreaType = SearchAreaType.Circle,
            GeoJson = """{"type":"Point","coordinates":[-3.533,50.723]}""",
            RadiusMetres = 500.0,
        };

        _writeContext.SearchAreas.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.SearchAreas.AsNoTracking().Single(a => a.Id == _saved.Id);

    It should_persist_the_radius = () => _loaded.RadiusMetres.ShouldEqual(500.0);
    It should_persist_the_circle_type = () => _loaded.AreaType.ShouldEqual(SearchAreaType.Circle);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Sector Persistence")]
class When_round_tripping_a_sector_through_the_database
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Sector _saved;
    static Sector _loaded;
    static Guid _searchAreaId;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _searchAreaId = Guid.NewGuid();
        var area = new SearchArea
        {
            Id = _searchAreaId,
            CaseId = Guid.NewGuid(),
            Name = "Zone A",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };
        _writeContext.SearchAreas.Add(area);

        _saved = new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = _searchAreaId,
            Name = "Sector Alpha",
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}""",
            AssignedToUserId = Guid.NewGuid(),
        };
        _writeContext.Sectors.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Sectors.AsNoTracking().Single(s => s.Id == _saved.Id);

    It should_persist_the_name = () => _loaded.Name.ShouldEqual("Sector Alpha");
    It should_persist_the_search_area_id = () => _loaded.SearchAreaId.ShouldEqual(_searchAreaId);
    It should_persist_the_geojson = () => _loaded.GeoJson.ShouldEqual(_saved.GeoJson);
    It should_persist_the_status = () => _loaded.Status.ShouldEqual(SectorStatus.NotStarted);
    It should_persist_the_assigned_user = () => _loaded.AssignedToUserId.ShouldEqual(_saved.AssignedToUserId);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("SearchArea Persistence")]
class When_loading_a_search_area_with_sectors
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static SearchArea _saved;
    static SearchArea _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        _saved = new SearchArea
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Name = "Zone B",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };
        _saved.Sectors.Add(new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = _saved.Id,
            Name = "S1",
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}""",
        });
        _saved.Sectors.Add(new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = _saved.Id,
            Name = "S2",
            GeoJson = """{"type":"Polygon","coordinates":[[[0.5,0],[1,0],[1,0.5],[0.5,0.5],[0.5,0]]]}""",
        });

        _writeContext.SearchAreas.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.SearchAreas
        .Include(a => a.Sectors)
        .AsNoTracking()
        .Single(a => a.Id == _saved.Id);

    It should_load_both_sectors = () => _loaded.Sectors.Count.ShouldEqual(2);
    It should_have_sector_s1 = () => _loaded.Sectors.ShouldContain(s => s.Name == "S1");
    It should_have_sector_s2 = () => _loaded.Sectors.ShouldContain(s => s.Name == "S2");

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("SearchArea Persistence")]
class When_deleting_a_search_area_cascades_sectors
{
    static SarDbContext _context;
    static SqliteConnection _connection;
    static Guid _areaId;

    Establish context_setup = () =>
    {
        (_context, _connection) = SarDbContextFactory.CreateInMemory();

        _areaId = Guid.NewGuid();
        var area = new SearchArea
        {
            Id = _areaId,
            CaseId = Guid.NewGuid(),
            Name = "Zone C",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };
        area.Sectors.Add(new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = _areaId,
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        });

        _context.SearchAreas.Add(area);
        _context.SaveChanges();
    };

    Because of = () =>
    {
        var area = _context.SearchAreas.Single(a => a.Id == _areaId);
        _context.SearchAreas.Remove(area);
        _context.SaveChanges();
    };

    It should_remove_the_area = () => _context.SearchAreas.Count().ShouldEqual(0);
    It should_cascade_delete_sectors = () => _context.Sectors.Count().ShouldEqual(0);

    Cleanup after = () =>
    {
        _context?.Dispose();
        _connection?.Dispose();
    };
}

[Subject("Sector Persistence")]
class When_persisting_sector_status_as_string
{
    static SarDbContext _writeContext;
    static SarDbContext _readContext;
    static SqliteConnection _connection;
    static Sector _saved;
    static Sector _loaded;

    Establish context = () =>
    {
        (_writeContext, _connection) = SarDbContextFactory.CreateInMemory();

        var area = new SearchArea
        {
            Id = Guid.NewGuid(),
            CaseId = Guid.NewGuid(),
            Name = "Zone D",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };
        _writeContext.SearchAreas.Add(area);

        _saved = new Sector
        {
            Id = Guid.NewGuid(),
            SearchAreaId = area.Id,
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _saved.StartSearch();
        _saved.Complete();

        _writeContext.Sectors.Add(_saved);
        _writeContext.SaveChanges();

        var readOptions = new DbContextOptionsBuilder<SarDbContext>()
            .UseSqlite(_connection)
            .Options;
        _readContext = new SarDbContext(readOptions);
    };

    Because of = () => _loaded = _readContext.Sectors.AsNoTracking().Single(s => s.Id == _saved.Id);

    It should_persist_completed_status = () => _loaded.Status.ShouldEqual(SectorStatus.Completed);

    Cleanup after = () =>
    {
        _readContext?.Dispose();
        _writeContext?.Dispose();
        _connection?.Dispose();
    };
}
