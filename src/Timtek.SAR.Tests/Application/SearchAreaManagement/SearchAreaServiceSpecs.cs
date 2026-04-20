using TA.Utils.Core;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.SearchAreaManagement;
using Timtek.SAR.Application.SearchAreaManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Application.SearchAreaManagement;

class SearchAreaServiceContextBuilder
{
    readonly IRepository<SearchArea, Guid> _searchAreaRepository = A.Fake<IRepository<SearchArea, Guid>>();
    readonly IRepository<Sector, Guid> _sectorRepository = A.Fake<IRepository<Sector, Guid>>();
    readonly IUnitOfWork _unitOfWork = A.Fake<IUnitOfWork>();
    readonly List<SearchArea> _existingAreas = [];

    public SearchAreaServiceContextBuilder WithExistingArea(SearchArea area)
    {
        _existingAreas.Add(area);
        A.CallTo(() => _searchAreaRepository.GetMaybe(area.Id)).Returns(area.AsMaybe());
        A.CallTo(() => _searchAreaRepository.GetAll()).Returns(_existingAreas);
        return this;
    }

    public SearchAreaServiceContextBuilder WithNoArea(Guid id)
    {
        A.CallTo(() => _searchAreaRepository.GetMaybe(id)).Returns(Maybe<SearchArea>.Empty);
        return this;
    }

    public SearchAreaServiceContextBuilder WithExistingSector(Sector sector)
    {
        A.CallTo(() => _sectorRepository.GetMaybe(sector.Id)).Returns(sector.AsMaybe());
        return this;
    }

    public SearchAreaServiceContextBuilder WithNoSector(Guid id)
    {
        A.CallTo(() => _sectorRepository.GetMaybe(id)).Returns(Maybe<Sector>.Empty);
        return this;
    }

    public ISearchAreaService Build() => new SearchAreaService(_searchAreaRepository, _sectorRepository, _unitOfWork);

    public IRepository<SearchArea, Guid> SearchAreaRepository => _searchAreaRepository;
    public IRepository<Sector, Guid> SectorRepository => _sectorRepository;
    public IUnitOfWork UnitOfWork => _unitOfWork;
}

static class SearchAreaTestHelper
{
    public const string SamplePolygonGeoJson =
        """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""";
    public const string SampleSectorGeoJson =
        """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}""";

    public static SearchArea CreateExistingArea(Guid? id = null, Guid? caseId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        CaseId = caseId ?? Guid.NewGuid(),
        Name = "Zone A",
        AreaType = SearchAreaType.Polygon,
        GeoJson = SamplePolygonGeoJson,
    };
}

// --- CreateAsync ---

[Subject("SearchArea Service")]
class When_creating_a_search_area
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static SearchAreaDto _result;
    static Guid _caseId;

    Establish context = () =>
    {
        _caseId = Guid.NewGuid();
        _context = new SearchAreaServiceContextBuilder();
        _service = _context.Build();
    };

    Because of = () => _result = _service.CreateAsync(new CreateSearchAreaRequest(
        _caseId, "River Bank", SearchAreaType.Polygon,
        SearchAreaTestHelper.SamplePolygonGeoJson)).GetAwaiter().GetResult();

    It should_return_the_area = () => _result.Name.ShouldEqual("River Bank");
    It should_have_the_case_id = () => _result.CaseId.ShouldEqual(_caseId);
    It should_have_polygon_type = () => _result.AreaType.ShouldEqual(SearchAreaType.Polygon);
    It should_have_empty_sectors = () => _result.Sectors.ShouldBeEmpty();
    It should_save_changes = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
    It should_add_to_repository = () =>
        A.CallTo(() => _context.SearchAreaRepository.Add(A<SearchArea>._)).MustHaveHappenedOnceExactly();
}

[Subject("SearchArea Service")]
class When_creating_a_circle_area_without_radius
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Exception _exception;

    Establish context = () =>
    {
        _context = new SearchAreaServiceContextBuilder();
        _service = _context.Build();
    };

    Because of = () => _exception = Catch.Exception(() => _service.CreateAsync(new CreateSearchAreaRequest(
        Guid.NewGuid(), "Circle", SearchAreaType.Circle,
        """{"type":"Point","coordinates":[0,0]}""")).GetAwaiter().GetResult());

    It should_throw = () => _exception.ShouldBeOfExactType<ArgumentException>();
}

[Subject("SearchArea Service")]
class When_creating_a_search_area_with_empty_name
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Exception _exception;

    Establish context = () =>
    {
        _context = new SearchAreaServiceContextBuilder();
        _service = _context.Build();
    };

    Because of = () => _exception = Catch.Exception(() => _service.CreateAsync(new CreateSearchAreaRequest(
        Guid.NewGuid(), "", SearchAreaType.Polygon,
        SearchAreaTestHelper.SamplePolygonGeoJson)).GetAwaiter().GetResult());

    It should_throw = () => _exception.ShouldBeOfExactType<ArgumentException>();
}

// --- GetByIdAsync ---

[Subject("SearchArea Service")]
class When_getting_an_existing_search_area_by_id
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Maybe<SearchAreaDto> _result;
    static Guid _areaId;

    Establish context = () =>
    {
        _areaId = Guid.NewGuid();
        var area = SearchAreaTestHelper.CreateExistingArea(id: _areaId);
        _context = new SearchAreaServiceContextBuilder().WithExistingArea(area);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetByIdAsync(_areaId).GetAwaiter().GetResult();

    It should_find_the_area = () => _result.Any().ShouldBeTrue();
    It should_have_the_correct_id = () => _result.Single().Id.ShouldEqual(_areaId);
}

[Subject("SearchArea Service")]
class When_getting_a_nonexistent_search_area
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Maybe<SearchAreaDto> _result;
    static Guid _id;

    Establish context = () =>
    {
        _id = Guid.NewGuid();
        _context = new SearchAreaServiceContextBuilder().WithNoArea(_id);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetByIdAsync(_id).GetAwaiter().GetResult();

    It should_return_empty = () => _result.Any().ShouldBeFalse();
}

// --- GetByCaseIdAsync ---

[Subject("SearchArea Service")]
class When_getting_search_areas_by_case_id
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static IReadOnlyList<SearchAreaDto> _result;
    static Guid _caseId;

    Establish context = () =>
    {
        _caseId = Guid.NewGuid();
        var area1 = SearchAreaTestHelper.CreateExistingArea(caseId: _caseId);
        var area2 = SearchAreaTestHelper.CreateExistingArea(caseId: _caseId);
        var otherArea = SearchAreaTestHelper.CreateExistingArea(caseId: Guid.NewGuid());
        _context = new SearchAreaServiceContextBuilder()
            .WithExistingArea(area1)
            .WithExistingArea(area2)
            .WithExistingArea(otherArea);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetByCaseIdAsync(_caseId).GetAwaiter().GetResult();

    It should_return_only_matching_areas = () => _result.Count.ShouldEqual(2);
    It should_all_belong_to_the_case = () => _result.ShouldEachConformTo(a => a.CaseId == _caseId);
}

// --- AddSectorAsync ---

[Subject("SearchArea Service")]
class When_adding_a_sector_to_a_search_area
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static SectorDto _result;
    static Guid _areaId;

    Establish context = () =>
    {
        _areaId = Guid.NewGuid();
        var area = SearchAreaTestHelper.CreateExistingArea(id: _areaId);
        _context = new SearchAreaServiceContextBuilder().WithExistingArea(area);
        _service = _context.Build();
    };

    Because of = () => _result = _service.AddSectorAsync(_areaId,
        new AddSectorRequest("Sector 1", SearchAreaTestHelper.SampleSectorGeoJson)).GetAwaiter().GetResult();

    It should_return_the_sector = () => _result.Name.ShouldEqual("Sector 1");
    It should_link_to_the_area = () => _result.SearchAreaId.ShouldEqual(_areaId);
    It should_default_to_not_started = () => _result.Status.ShouldEqual(SectorStatus.NotStarted);
    It should_add_to_sector_repository = () =>
        A.CallTo(() => _context.SectorRepository.Add(A<Sector>._)).MustHaveHappenedOnceExactly();
    It should_save_changes = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("SearchArea Service")]
class When_adding_a_sector_to_a_nonexistent_area
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Exception _exception;
    static Guid _id;

    Establish context = () =>
    {
        _id = Guid.NewGuid();
        _context = new SearchAreaServiceContextBuilder().WithNoArea(_id);
        _service = _context.Build();
    };

    Because of = () => _exception = Catch.Exception(() => _service.AddSectorAsync(_id,
        new AddSectorRequest("S1", SearchAreaTestHelper.SampleSectorGeoJson)).GetAwaiter().GetResult());

    It should_throw = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

// --- UpdateSectorStatusAsync ---

[Subject("SearchArea Service")]
class When_updating_sector_status_to_in_progress
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static SectorDto _result;
    static Guid _sectorId;

    Establish context = () =>
    {
        _sectorId = Guid.NewGuid();
        var sector = new Sector
        {
            Id = _sectorId,
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = SearchAreaTestHelper.SampleSectorGeoJson,
        };
        _context = new SearchAreaServiceContextBuilder().WithExistingSector(sector);
        _service = _context.Build();
    };

    Because of = () => _result = _service.UpdateSectorStatusAsync(_sectorId, SectorStatus.InProgress).GetAwaiter().GetResult();

    It should_be_in_progress = () => _result.Status.ShouldEqual(SectorStatus.InProgress);
    It should_save_changes = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("SearchArea Service")]
class When_updating_status_of_nonexistent_sector
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Exception _exception;
    static Guid _id;

    Establish context = () =>
    {
        _id = Guid.NewGuid();
        _context = new SearchAreaServiceContextBuilder().WithNoSector(_id);
        _service = _context.Build();
    };

    Because of = () => _exception = Catch.Exception(() =>
        _service.UpdateSectorStatusAsync(_id, SectorStatus.InProgress).GetAwaiter().GetResult());

    It should_throw = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

// --- AssignSectorAsync ---

[Subject("SearchArea Service")]
class When_assigning_a_sector_to_a_user
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static SectorDto _result;
    static Guid _sectorId;
    static Guid _userId;

    Establish context = () =>
    {
        _sectorId = Guid.NewGuid();
        _userId = Guid.NewGuid();
        var sector = new Sector
        {
            Id = _sectorId,
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = SearchAreaTestHelper.SampleSectorGeoJson,
        };
        _context = new SearchAreaServiceContextBuilder().WithExistingSector(sector);
        _service = _context.Build();
    };

    Because of = () => _result = _service.AssignSectorAsync(_sectorId, _userId).GetAwaiter().GetResult();

    It should_assign_the_user = () => _result.AssignedToUserId.ShouldEqual(_userId);
    It should_save_changes = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}

[Subject("SearchArea Service")]
class When_unassigning_a_sector
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static SectorDto _result;
    static Guid _sectorId;

    Establish context = () =>
    {
        _sectorId = Guid.NewGuid();
        var sector = new Sector
        {
            Id = _sectorId,
            SearchAreaId = Guid.NewGuid(),
            Name = "S1",
            GeoJson = SearchAreaTestHelper.SampleSectorGeoJson,
            AssignedToUserId = Guid.NewGuid(),
        };
        _context = new SearchAreaServiceContextBuilder().WithExistingSector(sector);
        _service = _context.Build();
    };

    Because of = () => _result = _service.AssignSectorAsync(_sectorId, null).GetAwaiter().GetResult();

    It should_clear_the_assignment = () => _result.AssignedToUserId.ShouldBeNull();
}

// --- DeleteAsync ---

[Subject("SearchArea Service")]
class When_deleting_a_search_area
{
    static SearchAreaServiceContextBuilder _context;
    static ISearchAreaService _service;
    static Guid _areaId;

    Establish context = () =>
    {
        _areaId = Guid.NewGuid();
        var area = SearchAreaTestHelper.CreateExistingArea(id: _areaId);
        _context = new SearchAreaServiceContextBuilder().WithExistingArea(area);
        _service = _context.Build();
    };

    Because of = () => _service.DeleteAsync(_areaId).GetAwaiter().GetResult();

    It should_remove_from_repository = () =>
        A.CallTo(() => _context.SearchAreaRepository.Remove(A<SearchArea>._)).MustHaveHappenedOnceExactly();
    It should_save_changes = () =>
        A.CallTo(() => _context.UnitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
}
