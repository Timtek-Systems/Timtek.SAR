using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Domain.Entities;

// --- SearchArea ---

[Subject("SearchArea")]
class When_creating_a_search_area
{
    static SearchArea _area;

    Because of = () => _area = new SearchArea
    {
        Id = Guid.NewGuid(),
        CaseId = Guid.NewGuid(),
        Name = "River Bank Zone",
        AreaType = SearchAreaType.Polygon,
        GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
    };

    It should_have_the_name = () => _area.Name.ShouldEqual("River Bank Zone");
    It should_have_polygon_type = () => _area.AreaType.ShouldEqual(SearchAreaType.Polygon);
    It should_have_empty_sectors = () => _area.Sectors.ShouldBeEmpty();
    It should_have_a_creation_timestamp = () => _area.CreatedAtUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
}

[Subject("SearchArea")]
class When_creating_a_circle_search_area
{
    static SearchArea _area;

    Because of = () => _area = new SearchArea
    {
        Id = Guid.NewGuid(),
        CaseId = Guid.NewGuid(),
        Name = "Last Seen Radius",
        AreaType = SearchAreaType.Circle,
        GeoJson = """{"type":"Point","coordinates":[-3.533,50.723]}""",
        RadiusMetres = 500.0,
    };

    It should_have_circle_type = () => _area.AreaType.ShouldEqual(SearchAreaType.Circle);
    It should_store_the_radius = () => _area.RadiusMetres.ShouldEqual(500.0);
}

[Subject("SearchArea")]
class When_adding_a_sector_to_a_search_area
{
    static SearchArea _area;
    static Sector _sector;
    static Guid _areaId;

    Establish context = () =>
    {
        _areaId = Guid.NewGuid();
        _area = new SearchArea
        {
            Id = _areaId,
            CaseId = Guid.NewGuid(),
            Name = "Zone A",
            AreaType = SearchAreaType.Polygon,
            GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
        };
    };

    Because of = () => _sector = _area.AddSector("Sector 1",
        """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}""");

    It should_add_the_sector_to_the_collection = () => _area.Sectors.Count.ShouldEqual(1);
    It should_set_the_sector_name = () => _sector.Name.ShouldEqual("Sector 1");
    It should_link_the_sector_to_the_area = () => _sector.SearchAreaId.ShouldEqual(_areaId);
    It should_default_to_not_started = () => _sector.Status.ShouldEqual(SectorStatus.NotStarted);
}

[Subject("SearchArea")]
class When_adding_a_sector_with_empty_name
{
    static SearchArea _area;
    static Exception _exception;

    Establish context = () => _area = new SearchArea
    {
        Id = Guid.NewGuid(),
        CaseId = Guid.NewGuid(),
        Name = "Zone A",
        AreaType = SearchAreaType.Polygon,
        GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
    };

    Because of = () => _exception = Catch.Exception(() => _area.AddSector("", """{"type":"Polygon"}"""));

    It should_throw = () => _exception.ShouldBeOfExactType<ArgumentException>();
}

[Subject("SearchArea")]
class When_adding_a_sector_with_empty_geojson
{
    static SearchArea _area;
    static Exception _exception;

    Establish context = () => _area = new SearchArea
    {
        Id = Guid.NewGuid(),
        CaseId = Guid.NewGuid(),
        Name = "Zone A",
        AreaType = SearchAreaType.Polygon,
        GeoJson = """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""",
    };

    Because of = () => _exception = Catch.Exception(() => _area.AddSector("Sector 1", "  "));

    It should_throw = () => _exception.ShouldBeOfExactType<ArgumentException>();
}

// --- Sector status transitions ---

[Subject("Sector")]
class When_starting_search_on_a_not_started_sector
{
    static Sector _sector;

    Establish context = () => _sector = new Sector
    {
        SearchAreaId = Guid.NewGuid(),
        Name = "S1",
        GeoJson = """{"type":"Polygon"}""",
    };

    Because of = () => _sector.StartSearch();

    It should_be_in_progress = () => _sector.Status.ShouldEqual(SectorStatus.InProgress);
}

[Subject("Sector")]
class When_completing_an_in_progress_sector
{
    static Sector _sector;

    Establish context = () =>
    {
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _sector.StartSearch();
    };

    Because of = () => _sector.Complete();

    It should_be_completed = () => _sector.Status.ShouldEqual(SectorStatus.Completed);
}

[Subject("Sector")]
class When_marking_an_in_progress_sector_needs_research
{
    static Sector _sector;

    Establish context = () =>
    {
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _sector.StartSearch();
    };

    Because of = () => _sector.MarkNeedsResearch();

    It should_need_research = () => _sector.Status.ShouldEqual(SectorStatus.NeedsResearch);
}

[Subject("Sector")]
class When_marking_a_completed_sector_needs_research
{
    static Sector _sector;

    Establish context = () =>
    {
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _sector.StartSearch();
        _sector.Complete();
    };

    Because of = () => _sector.MarkNeedsResearch();

    It should_need_research = () => _sector.Status.ShouldEqual(SectorStatus.NeedsResearch);
}

[Subject("Sector")]
class When_restarting_search_on_a_needs_research_sector
{
    static Sector _sector;

    Establish context = () =>
    {
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _sector.StartSearch();
        _sector.MarkNeedsResearch();
    };

    Because of = () => _sector.StartSearch();

    It should_be_in_progress = () => _sector.Status.ShouldEqual(SectorStatus.InProgress);
}

[Subject("Sector")]
class When_trying_to_complete_a_not_started_sector
{
    static Sector _sector;
    static Exception _exception;

    Establish context = () => _sector = new Sector
    {
        SearchAreaId = Guid.NewGuid(),
        Name = "S1",
        GeoJson = """{"type":"Polygon"}""",
    };

    Because of = () => _exception = Catch.Exception(() => _sector.Complete());

    It should_throw_invalid_operation = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

[Subject("Sector")]
class When_trying_to_start_a_completed_sector
{
    static Sector _sector;
    static Exception _exception;

    Establish context = () =>
    {
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
        _sector.StartSearch();
        _sector.Complete();
    };

    Because of = () => _exception = Catch.Exception(() => _sector.StartSearch());

    It should_throw_invalid_operation = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

[Subject("Sector")]
class When_assigning_a_sector_to_a_user
{
    static Sector _sector;
    static Guid _userId;

    Establish context = () =>
    {
        _userId = Guid.NewGuid();
        _sector = new Sector
        {
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = """{"type":"Polygon"}""",
        };
    };

    Because of = () => _sector.AssignedToUserId = _userId;

    It should_store_the_user_id = () => _sector.AssignedToUserId.ShouldEqual(_userId);
}
