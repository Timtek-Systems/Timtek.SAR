using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timtek.SAR.Application.OrganisationManagement.Dtos;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Api.Endpoints;

static class OrganisationApiTestFactory
{
    public static WebApplicationFactory<Program> Create()
    {
        var dbName = $"test_{Guid.NewGuid():N}.db";
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<SarDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<SarDbContext>(options =>
                        options.UseSqlite($"DataSource={dbName}"));
                });
            });
    }
}

[Subject("Organisation API")]
class When_creating_an_organisation_via_the_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static OrganisationResponse _body;

    Establish context = () =>
    {
        _factory = OrganisationApiTestFactory.Create();
        _client = _factory.CreateClient();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/organisations",
            new CreateOrganisationRequest("Devon SAR")).GetAwaiter().GetResult();
        _body = _response.Content.ReadFromJsonAsync<OrganisationResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_created_status = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    It should_return_the_organisation_name = () => _body.Name.ShouldEqual("Devon SAR");
    It should_return_a_valid_id = () => _body.Id.ShouldNotEqual(Guid.Empty);
    It should_have_default_ao_radius = () => _body.DefaultAoRadiusKm.ShouldEqual(25.0);
    It should_include_location_header = () =>
        _response.Headers.Location!.ToString().ShouldContain(_body.Id.ToString());
    It should_include_correlation_id_header = () =>
        _response.Headers.Contains("X-Correlation-ID").ShouldBeTrue();

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Organisation API")]
class When_getting_an_organisation_via_the_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static OrganisationResponse _created;
    static HttpResponseMessage _response;
    static OrganisationResponse _body;

    Establish context = () =>
    {
        _factory = OrganisationApiTestFactory.Create();
        _client = _factory.CreateClient();

        var createResponse = _client.PostAsJsonAsync("/api/organisations",
            new CreateOrganisationRequest("Devon SAR")).GetAwaiter().GetResult();
        _created = createResponse.Content.ReadFromJsonAsync<OrganisationResponse>().GetAwaiter().GetResult()!;
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/organisations/{_created.Id}").GetAwaiter().GetResult();
        _body = _response.Content.ReadFromJsonAsync<OrganisationResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_ok_status = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_return_the_correct_name = () => _body.Name.ShouldEqual("Devon SAR");
    It should_return_the_correct_id = () => _body.Id.ShouldEqual(_created.Id);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Organisation API")]
class When_getting_a_nonexistent_organisation_via_the_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;

    Establish context = () =>
    {
        _factory = OrganisationApiTestFactory.Create();
        _client = _factory.CreateClient();
    };

    Because of = () =>
        _response = _client.GetAsync($"/api/organisations/{Guid.NewGuid()}").GetAwaiter().GetResult();

    It should_return_not_found = () => _response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Organisation API")]
class When_updating_organisation_settings_via_the_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static OrganisationResponse _created;
    static HttpResponseMessage _updateResponse;
    static OrganisationResponse _afterUpdate;

    Establish context = () =>
    {
        _factory = OrganisationApiTestFactory.Create();
        _client = _factory.CreateClient();

        var createResponse = _client.PostAsJsonAsync("/api/organisations",
            new CreateOrganisationRequest("Devon SAR")).GetAwaiter().GetResult();
        _created = createResponse.Content.ReadFromJsonAsync<OrganisationResponse>().GetAwaiter().GetResult()!;
    };

    Because of = () =>
    {
        _updateResponse = _client.PutAsJsonAsync($"/api/organisations/{_created.Id}/settings",
            new UpdateOrganisationSettingsRequest(
                DefaultAoRadiusKm: 30.0,
                DefaultReputationScore: 60,
                MinimumReputationThreshold: 15,
                ExtendedNotificationRadiusKm: 75.0,
                KeeperInvitationExpiryDays: 14,
                KeeperRetentionDays: 180)).GetAwaiter().GetResult();

        var getResponse = _client.GetAsync($"/api/organisations/{_created.Id}").GetAwaiter().GetResult();
        _afterUpdate = getResponse.Content.ReadFromJsonAsync<OrganisationResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_no_content = () => _updateResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    It should_persist_the_ao_radius = () => _afterUpdate.DefaultAoRadiusKm.ShouldEqual(30.0);
    It should_persist_the_reputation_score = () => _afterUpdate.DefaultReputationScore.ShouldEqual(60);
    It should_persist_the_minimum_threshold = () => _afterUpdate.MinimumReputationThreshold.ShouldEqual(15);
    It should_persist_the_notification_radius = () => _afterUpdate.ExtendedNotificationRadiusKm.ShouldEqual(75.0);
    It should_persist_the_invitation_expiry = () => _afterUpdate.KeeperInvitationExpiryDays.ShouldEqual(14);
    It should_persist_the_retention_days = () => _afterUpdate.KeeperRetentionDays.ShouldEqual(180);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}
