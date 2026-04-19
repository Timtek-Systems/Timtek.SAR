using System.Security.Claims;
using Timtek.SAR.Application.CaseManagement;
using Timtek.SAR.Application.CaseManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Api.Endpoints;

public static class CaseEndpoints
{
    public static RouteGroupBuilder MapCaseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cases").RequireAuthorization();

        group.MapPost("/", async (CreateCaseRequest request, ICaseService service) =>
        {
            var result = await service.CreateAsync(request);
            return Results.Created($"/api/cases/{result.Id}", result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICaseService service) =>
        {
            var result = await service.GetAsync(id);
            return result.Any() ? Results.Ok(result.Single()) : Results.NotFound();
        });

        group.MapGet("/organisation/{organisationId:guid}", async (Guid organisationId, ICaseService service) =>
        {
            var result = await service.GetByOrganisationAsync(organisationId);
            return Results.Ok(result);
        });

        group.MapPost("/{id:guid}/triage", async (Guid id, ICaseService service) =>
        {
            await service.TriageAsync(id);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/start-search", async (Guid id, ICaseService service) =>
        {
            await service.StartActiveSearchAsync(id);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/suspend", async (Guid id, ICaseService service) =>
        {
            await service.SuspendAsync(id);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/resolve", async (Guid id, ResolveRequest request, ICaseService service) =>
        {
            await service.ResolveAsync(id, request.Outcome);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/priority", async (Guid id, SetPriorityRequest request, ICaseService service) =>
        {
            await service.SetPriorityAsync(id, request.Priority);
            return Results.NoContent();
        });

        group.MapPost("/{id:guid}/notes", async (Guid id, AddNoteRequest request, HttpContext httpContext, ICaseService service) =>
        {
            var userId = Guid.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await service.AddNoteAsync(id, userId, request.Note);
            return Results.NoContent();
        });

        group.MapGet("/{id:guid}/activity-log", async (Guid id, ICaseService service) =>
        {
            var result = await service.GetActivityLogAsync(id);
            return Results.Ok(result);
        });

        return group;
    }
}

public sealed record ResolveRequest(CaseOutcome Outcome);
public sealed record SetPriorityRequest(CasePriority Priority);
public sealed record AddNoteRequest(string Note);
