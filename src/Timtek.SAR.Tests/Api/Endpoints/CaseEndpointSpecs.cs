using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Api.Endpoints;

// --- Case creation ---

[Subject("Case Endpoints")]
class When_creating_a_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static CaseDto _result;
    static Guid _orgId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        var request = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Dog", "Labrador", "Golden", "Large", null, null,
            50.7184, -3.5339, DateTime.UtcNow, "Jane", "07700900123", "jane@example.com", null, null);
        _response = _client.PostAsJsonAsync("/api/cases", request).GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
    };

    It should_return_created = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    It should_have_an_id = () => _result.Id.ShouldNotEqual(Guid.Empty);
    It should_have_reported_status = () => _result.Status.ShouldEqual(CaseStatus.Reported);
    It should_have_a_reference_number = () => _result.ReferenceNumber.ShouldNotBeEmpty();
    It should_have_the_species = () => _result.AnimalSpecies.ShouldEqual("Dog");

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Get case ---

[Subject("Case Endpoints")]
class When_getting_an_existing_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static CaseDto _result;
    static Guid _orgId;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();

        var request = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Cat", null, null, null, null, null,
            51.5, -0.1, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var createResponse = _client.PostAsJsonAsync("/api/cases", request).GetAwaiter().GetResult();
        var created = createResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
        _caseId = created.Id;
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/cases/{_caseId}").GetAwaiter().GetResult();
        _result = _response.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_have_the_correct_id = () => _result.Id.ShouldEqual(_caseId);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Case Endpoints")]
class When_getting_a_nonexistent_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static Guid _orgId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();
    };

    Because of = () => _response = _client.GetAsync($"/api/cases/{Guid.NewGuid()}").GetAwaiter().GetResult();

    It should_return_not_found = () => _response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Status workflow via API ---

[Subject("Case Endpoints")]
class When_triaging_a_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _triageResponse;
    static CaseDto _result;
    static Guid _orgId;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();

        var request = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var createResponse = _client.PostAsJsonAsync("/api/cases", request).GetAwaiter().GetResult();
        _caseId = createResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;
    };

    Because of = () =>
    {
        _triageResponse = _client.PostAsync($"/api/cases/{_caseId}/triage", null).GetAwaiter().GetResult();
        var triageBody = _triageResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        System.IO.File.WriteAllText(@"Z:\Timtek\Timtek.SAR\triage_debug.txt", $"Status: {_triageResponse.StatusCode}\nBody: {triageBody}");
        if (_triageResponse.IsSuccessStatusCode)
        {
            var getResponse = _client.GetAsync($"/api/cases/{_caseId}").GetAwaiter().GetResult();
            _result = getResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
        }
    };

    It should_return_no_content = () => _triageResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    It should_change_status_to_triaged = () => _result.Status.ShouldEqual(CaseStatus.Triaged);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Case Endpoints")]
class When_resolving_a_case_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _resolveResponse;
    static CaseDto _result;
    static Guid _orgId;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();

        var request = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var createResponse = _client.PostAsJsonAsync("/api/cases", request).GetAwaiter().GetResult();
        _caseId = createResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;

        _client.PostAsync($"/api/cases/{_caseId}/triage", null).GetAwaiter().GetResult();
        _client.PostAsync($"/api/cases/{_caseId}/start-search", null).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _resolveResponse = _client.PostAsJsonAsync($"/api/cases/{_caseId}/resolve",
            new { Outcome = CaseOutcome.Reunited }).GetAwaiter().GetResult();
        var getResponse = _client.GetAsync($"/api/cases/{_caseId}").GetAwaiter().GetResult();
        _result = getResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
    };

    It should_return_no_content = () => _resolveResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    It should_change_status_to_resolved = () => _result.Status.ShouldEqual(CaseStatus.Resolved);
    It should_set_the_outcome = () => _result.Outcome.ShouldEqual(CaseOutcome.Reunited);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Priority ---

[Subject("Case Endpoints")]
class When_setting_priority_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _priorityResponse;
    static CaseDto _result;
    static Guid _orgId;
    static Guid _caseId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();

        var request = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner", null, "owner@example.com", null, null);
        var createResponse = _client.PostAsJsonAsync("/api/cases", request).GetAwaiter().GetResult();
        _caseId = createResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!.Id;
    };

    Because of = () =>
    {
        _priorityResponse = _client.PostAsJsonAsync($"/api/cases/{_caseId}/priority",
            new { Priority = CasePriority.Critical }).GetAwaiter().GetResult();
        var getResponse = _client.GetAsync($"/api/cases/{_caseId}").GetAwaiter().GetResult();
        _result = getResponse.Content.ReadFromJsonAsync<CaseDto>().GetAwaiter().GetResult()!;
    };

    It should_return_no_content = () => _priorityResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    It should_set_the_priority = () => _result.Priority.ShouldEqual(CasePriority.Critical);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Unauthenticated access ---

[Subject("Case Endpoints")]
class When_accessing_cases_without_authentication
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
    };

    Because of = () => _response = _client.GetAsync("/api/cases/" + Guid.NewGuid()).GetAwaiter().GetResult();

    It should_return_unauthorized = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Get cases by organisation ---

[Subject("Case Endpoints")]
class When_getting_cases_by_organisation_via_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static CaseDto[] _results;
    static Guid _orgId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _orgId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _orgId).GetAwaiter().GetResult();

        var request1 = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Dog", null, null, null, null, null,
            50.7, -3.5, DateTime.UtcNow, "Owner1", null, "o1@example.com", null, null);
        var request2 = new CreateCaseRequest(
            _orgId, Guid.NewGuid(), "Cat", null, null, null, null, null,
            51.5, -0.1, DateTime.UtcNow, "Owner2", null, "o2@example.com", null, null);
        _client.PostAsJsonAsync("/api/cases", request1).GetAwaiter().GetResult();
        _client.PostAsJsonAsync("/api/cases", request2).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/cases/organisation/{_orgId}").GetAwaiter().GetResult();
        _results = _response.Content.ReadFromJsonAsync<CaseDto[]>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_return_both_cases = () => _results.Length.ShouldEqual(2);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}
