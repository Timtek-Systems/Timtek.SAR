using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timtek.SAR.Application.UserManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Tests.Api.Endpoints;

static class AuthApiTestFactory
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

    public static async Task<Guid> SeedOrganisationAsync(WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SarDbContext>();
        var org = new Organisation { Id = Guid.NewGuid(), Name = "Test Org" };
        db.Organisations.Add(org);
        await db.SaveChangesAsync();
        return org.Id;
    }

    public static async Task SeedRolesAsync(WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        string[] roles = ["Administrator", "CaseManager", "TeamLead", "DroneOperator", "GroundSearcher"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
        }
    }

    public static async Task<HttpClient> CreateAuthenticatedAdminClientAsync(WebApplicationFactory<Program> factory, Guid organisationId)
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true,
        });

        // Register an admin user
        await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("admin@test.com", "Admin User", "P@ssw0rd1", organisationId));

        // Assign admin role via scope
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync("admin@test.com");
        await userManager.AddToRoleAsync(user!, "Administrator");

        // Login
        await client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("admin@test.com", "P@ssw0rd1"));

        return client;
    }
}

// --- Registration ---

[Subject("Auth API")]
class When_registering_a_user_via_the_api
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static RegisterResponse _body;
    static Guid _organisationId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("john@example.com", "John Doe", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
        _body = _response.Content.ReadFromJsonAsync<RegisterResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_created_status = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Created);
    It should_succeed = () => _body.Succeeded.ShouldBeTrue();
    It should_return_a_user_id = () => _body.UserId.ShouldNotBeNull();

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Auth API")]
class When_registering_a_user_with_weak_password
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static Guid _organisationId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("john@example.com", "John Doe", "short", _organisationId)).GetAwaiter().GetResult();
    };

    It should_return_bad_request = () => _response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- Login ---

[Subject("Auth API")]
class When_logging_in_with_valid_credentials
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static Guid _organisationId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = true });
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();

        _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("john@example.com", "John Doe", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("john@example.com", "P@ssw0rd1")).GetAwaiter().GetResult();
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Auth API")]
class When_logging_in_with_wrong_password
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static Guid _organisationId;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();

        _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("john@example.com", "John Doe", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("john@example.com", "WrongPass1")).GetAwaiter().GetResult();
    };

    It should_return_unauthorized = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Auth API")]
class When_logging_in_with_nonexistent_email
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("nobody@example.com", "P@ssw0rd1")).GetAwaiter().GetResult();
    };

    It should_return_unauthorized = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

// --- User management (admin-only) ---

[Subject("User API")]
class When_getting_a_user_as_admin
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static UserResponse _body;
    static Guid _organisationId;
    static RegisterResponse _registered;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _organisationId).GetAwaiter().GetResult();

        // Register a second user to look up
        var regResponse = _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("target@example.com", "Target User", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
        _registered = regResponse.Content.ReadFromJsonAsync<RegisterResponse>().GetAwaiter().GetResult()!;
    };

    Because of = () =>
    {
        _response = _client.GetAsync($"/api/users/{_registered.UserId}").GetAwaiter().GetResult();
        _body = _response.Content.ReadFromJsonAsync<UserResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_return_the_correct_email = () => _body.Email.ShouldEqual("target@example.com");
    It should_return_the_correct_display_name = () => _body.DisplayName.ShouldEqual("Target User");

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("User API")]
class When_getting_a_user_without_authentication
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _client = _factory.CreateClient();
    };

    Because of = () =>
        _response = _client.GetAsync($"/api/users/{Guid.NewGuid()}").GetAwaiter().GetResult();

    It should_return_unauthorized = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("User API")]
class When_assigning_a_role_as_admin
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _response;
    static Guid _organisationId;
    static RegisterResponse _registered;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _organisationId).GetAwaiter().GetResult();

        var regResponse = _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("target@example.com", "Target User", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
        _registered = regResponse.Content.ReadFromJsonAsync<RegisterResponse>().GetAwaiter().GetResult()!;
    };

    Because of = () =>
    {
        _response = _client.PostAsJsonAsync($"/api/users/{_registered.UserId}/roles",
            new AssignRoleRequest("CaseManager")).GetAwaiter().GetResult();
    };

    It should_return_ok = () => _response.StatusCode.ShouldEqual(HttpStatusCode.OK);

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("User API")]
class When_deactivating_a_user_as_admin
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _client;
    static HttpResponseMessage _deactivateResponse;
    static UserResponse _afterDeactivate;
    static Guid _organisationId;
    static RegisterResponse _registered;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _client = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _organisationId).GetAwaiter().GetResult();

        var regResponse = _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("target@example.com", "Target User", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
        _registered = regResponse.Content.ReadFromJsonAsync<RegisterResponse>().GetAwaiter().GetResult()!;
    };

    Because of = () =>
    {
        _deactivateResponse = _client.PutAsync($"/api/users/{_registered.UserId}/deactivate", null).GetAwaiter().GetResult();
        var getResponse = _client.GetAsync($"/api/users/{_registered.UserId}").GetAwaiter().GetResult();
        _afterDeactivate = getResponse.Content.ReadFromJsonAsync<UserResponse>().GetAwaiter().GetResult()!;
    };

    It should_return_ok = () => _deactivateResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
    It should_mark_user_inactive = () => _afterDeactivate.IsActive.ShouldBeFalse();

    Cleanup after = () =>
    {
        _client?.Dispose();
        _factory?.Dispose();
    };
}

[Subject("Auth API")]
class When_a_deactivated_user_tries_to_login
{
    static WebApplicationFactory<Program> _factory;
    static HttpClient _adminClient;
    static HttpClient _loginClient;
    static HttpResponseMessage _response;
    static Guid _organisationId;
    static RegisterResponse _registered;

    Establish context = () =>
    {
        _factory = AuthApiTestFactory.Create();
        _organisationId = AuthApiTestFactory.SeedOrganisationAsync(_factory).GetAwaiter().GetResult();
        AuthApiTestFactory.SeedRolesAsync(_factory).GetAwaiter().GetResult();
        _adminClient = AuthApiTestFactory.CreateAuthenticatedAdminClientAsync(_factory, _organisationId).GetAwaiter().GetResult();

        // Register a user
        var regResponse = _adminClient.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("target@example.com", "Target User", "P@ssw0rd1", _organisationId)).GetAwaiter().GetResult();
        _registered = regResponse.Content.ReadFromJsonAsync<RegisterResponse>().GetAwaiter().GetResult()!;

        // Deactivate
        _adminClient.PutAsync($"/api/users/{_registered.UserId}/deactivate", null).GetAwaiter().GetResult();

        _loginClient = _factory.CreateClient();
    };

    Because of = () =>
    {
        _response = _loginClient.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("target@example.com", "P@ssw0rd1")).GetAwaiter().GetResult();
    };

    It should_return_unauthorized = () => _response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

    Cleanup after = () =>
    {
        _loginClient?.Dispose();
        _adminClient?.Dispose();
        _factory?.Dispose();
    };
}
