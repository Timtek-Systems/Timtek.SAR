using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Timtek.SAR.Application.UserManagement;
using Timtek.SAR.Application.UserManagement.Dtos;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterRequest request, IUserService userService) =>
        {
            var result = await userService.RegisterAsync(new RegisterUserRequest(
                request.Email, request.DisplayName, request.Passphrase, request.OrganisationId));

            return result.Succeeded
                ? Results.Created($"/api/users/{result.UserId}", new RegisterResponse(true, result.UserId))
                : Results.ValidationProblem(
                    new Dictionary<string, string[]> { [""] = result.Errors!.ToArray() });
        });

        group.MapPost("/login", async (LoginRequest request, UserManager<ApplicationUser> userManager, HttpContext httpContext) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || !user.IsActive)
                return Results.Unauthorized();

            var passphraseValid = await userManager.CheckPasswordAsync(user, request.Passphrase);
            if (!passphraseValid)
                return Results.Unauthorized();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Email, user.Email!),
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

            return Results.Ok();
        });

        group.MapPost("/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Results.Ok();
        });

        return group;
    }
}
