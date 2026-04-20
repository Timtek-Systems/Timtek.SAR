using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Application.SearchAreaManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Api.Endpoints;

// --- Search area creation ---

[Subject("SearchArea Endpoints")]
class When_creating_a_search_area_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static SearchAreaDto _result;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        _caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;
    };

    Because of = () =>
    {
        var request = new CreateSearchAreaRequest(
            _caseId, "River Zone", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""");
        _response = _client.PostAsJsonAsync("/api/search-areas", request).GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!;
    };

    It should_return_created = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    It should_have_an_id = () => _result.Id.ShouldNotEqual(Guid.Empty);
    It should_have_the_name = () => _result.Name.ShouldEqual("River Zone");
    It should_have_the_case_id = () => _result.CaseId.ShouldEqual(_caseId);
    It should_have_polygon_type = () => _result.AreaType.ShouldEqual(SearchAreaType.Polygon);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Get search area ---

[Subject("SearchArea Endpoints")]
class When_getting_an_existing_search_area_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static SearchAreaDto _result;
    static Guid _areaId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Cat", null, null, null, null, null,
            51.5, -0.1, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        var caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        var areaRequest = new CreateSearchAreaRequest(
            caseId, "Zone A", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""");
        var areaResponse = _client.PostAsJsonAsync("/api/search-areas", areaRequest).GetAwaiter().GetResult();
        _areaId = areaResponse.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!.Id;
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/search-areas/{_areaId}").GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_have_the_correct_id = () => _result.Id.ShouldEqual(_areaId);
    It should_have_the_name = () => _result.Name.ShouldEqual("Zone A");

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("SearchArea Endpoints")]
class When_getting_a_nonexistent_search_area_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();
    };

    Because of = () => _response = _client.GetAsync($"/api/search-areas/{Guid.NewGuid()}").GetAwaiter().GetResult();

    It should_return_not_found = () => _response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Get by case ---

[Subject("SearchArea Endpoints")]
class When_getting_search_areas_by_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static SearchAreaDto[] _result;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        _caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        _client.PostAsJsonAsync("/api/search-areas", new CreateSearchAreaRequest(
            _caseId, "Zone 1", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""")).GetAwaiter().GetResult();
        _client.PostAsJsonAsync("/api/search-areas", new CreateSearchAreaRequest(
            _caseId, "Zone 2", SearchAreaType.Circle,
            """{"type":"Point","coordinates":[0,0]}""", 500)).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/search-areas/case/{_caseId}").GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<SearchAreaDto[]>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_return_two_areas = () => _result.Length.ShouldEqual(2);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Add sector ---

[Subject("SearchArea Endpoints")]
class When_adding_a_sector_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static SectorDto _result;
    static Guid _areaId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        var caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        var areaRequest = new CreateSearchAreaRequest(
            caseId, "Zone A", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""");
        var areaResponse = _client.PostAsJsonAsync("/api/search-areas", areaRequest).GetAwaiter().GetResult();
        _areaId = areaResponse.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!.Id;
    };

    Because of = () =>
    {
        var request = new AddSectorRequest("Sector Alpha",
            """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}""");
        _response = _client.PostAsJsonAsync($"/api/search-areas/{_areaId}/sectors", request).GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<SectorDto>().GetAwaiter().GetResult()!;
    };

    It should_return_created = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    It should_have_the_name = () => _result.Name.ShouldEqual("Sector Alpha");
    It should_be_not_started = () => _result.Status.ShouldEqual(SectorStatus.NotStarted);
    It should_link_to_the_area = () => _result.SearchAreaId.ShouldEqual(_areaId);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Update sector status ---

[Subject("SearchArea Endpoints")]
class When_updating_sector_status_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static SectorDto _result;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        var caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        var areaResponse = _client.PostAsJsonAsync("/api/search-areas", new CreateSearchAreaRequest(
            caseId, "Zone A", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""")).GetAwaiter().GetResult();
        var areaId = areaResponse.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!.Id;

        var sectorResponse = _client.PostAsJsonAsync($"/api/search-areas/{areaId}/sectors",
            new AddSectorRequest("S1", """{"type":"Polygon","coordinates":[[[0,0],[0.5,0],[0.5,0.5],[0,0.5],[0,0]]]}"""))
            .GetAwaiter().GetResult();
        var sectorId = sectorResponse.Content.ReadFromJsonAsync<SectorDto>().GetAwaiter().GetResult()!.Id;

        _response = _client.PutAsJsonAsync($"/api/search-areas/sectors/{sectorId}/status",
            new UpdateSectorStatusRequest(SectorStatus.InProgress)).GetAwaiter().GetResult();
    };

    Because of = () => _result = _response.Content.ReadFromJsonAsync<SectorDto>().GetAwaiter().GetResult()!;

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_be_in_progress = () => _result.Status.ShouldEqual(SectorStatus.InProgress);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Delete search area ---

[Subject("SearchArea Endpoints")]
class When_deleting_a_search_area_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _deleteResponse;
    static HttpResponseMessage _getAfterResponse;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        var caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        var areaResponse = _client.PostAsJsonAsync("/api/search-areas", new CreateSearchAreaRequest(
            caseId, "Zone Delete", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""")).GetAwaiter().GetResult();
        var areaId = areaResponse.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!.Id;

        _deleteResponse = _client.DeleteAsync($"/api/search-areas/{areaId}").GetAwaiter().GetResult();
        _getAfterResponse = _client.GetAsync($"/api/search-areas/{areaId}").GetAwaiter().GetResult();
    };

    Because of = () => { };

    It should_return_no_content = () => _deleteResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    It should_not_be_found_after = () => _getAfterResponse.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Export GeoJSON ---

[Subject("SearchArea Endpoints")]
class When_exporting_a_search_area_as_geojson_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static string _content;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        var orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, orgId).GetAwaiter().GetResult();

        var caseRequest = new CreateCaseRequest(
            orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var caseResponse = _client.PostAsJsonAsync("/api/cases", caseRequest).GetAwaiter().GetResult();
        var caseId = caseResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        var areaResponse = _client.PostAsJsonAsync("/api/search-areas", new CreateSearchAreaRequest(
            caseId, "Export Zone", SearchAreaType.Polygon,
            """{"type":"Polygon","coordinates":[[[0,0],[1,0],[1,1],[0,1],[0,0]]]}""")).GetAwaiter().GetResult();
        var areaId = areaResponse.Content.ReadFromJsonAsync<SearchAreaDto>().GetAwaiter().GetResult()!.Id;

        _response = _client.GetAsync($"/api/search-areas/{areaId}/export").GetAwaiter().GetResult();
    };

    Because of = () => _content = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_be_geojson_content_type = () => _response.Content.Headers.ContentType!.MediaType.ShouldEqual("application/geo+json");
    It should_contain_feature_collection = () => _content.ShouldContain("FeatureCollection");

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}
