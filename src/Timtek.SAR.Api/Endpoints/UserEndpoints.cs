using Timtek.SAR.Application.UserManagement;
using Timtek.SAR.Application.UserManagement.Dtos;

namespace Timtek.SAR.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").RequireAuthorization("Administrator");

        group.MapGet("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var result = await userService.GetByIdAsync(id);
            return result.Any()
                ? Results.Ok(MapToResponse(result.Single()))
                : Results.NotFound();
        });

        group.MapGet("/by-organisation/{organisationId:guid}", async (Guid organisationId, IUserService userService) =>
        {
            var users = await userService.GetByOrganisationAsync(organisationId);
            return Results.Ok(users.Select(MapToResponse).ToList());
        });

        group.MapPost("/{id:guid}/roles", async (Guid id, AssignRoleRequest request, IUserService userService) =>
        {
            var result = await userService.AssignRoleAsync(id, request.Role);
            return result ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}/roles/{role}", async (Guid id, string role, IUserService userService) =>
        {
            var result = await userService.RevokeRoleAsync(id, role);
            return result ? Results.Ok() : Results.NotFound();
        });

        group.MapPut("/{id:guid}/deactivate", async (Guid id, IUserService userService) =>
        {
            var result = await userService.DeactivateAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });

        group.MapPut("/{id:guid}/activate", async (Guid id, IUserService userService) =>
        {
            var result = await userService.ActivateAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        });

        return group;
    }

    static UserResponse MapToResponse(UserDto dto) =>
        new(dto.Id, dto.Email, dto.DisplayName, dto.OrganisationId, dto.IsActive, dto.Roles);
}
