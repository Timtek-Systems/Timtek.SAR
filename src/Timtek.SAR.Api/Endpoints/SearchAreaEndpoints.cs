using Timtek.SAR.Application.SearchAreaManagement;
using Timtek.SAR.Application.SearchAreaManagement.Dtos;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Api.Endpoints;

public static class SearchAreaEndpoints
{
    public static RouteGroupBuilder MapSearchAreaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/search-areas").RequireAuthorization();

        group.MapPost("/", async (CreateSearchAreaRequest request, ISearchAreaService service) =>
        {
            var result = await service.CreateAsync(request);
            return Results.Created($"/api/search-areas/{result.Id}", result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISearchAreaService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.Any() ? Results.Ok(result.Single()) : Results.NotFound();
        });

        group.MapGet("/case/{caseId:guid}", async (Guid caseId, ISearchAreaService service) =>
        {
            var result = await service.GetByCaseIdAsync(caseId);
            return Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISearchAreaService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });

        group.MapGet("/{id:guid}/export", async (Guid id, ISearchAreaService service) =>
        {
            var geoJson = await service.ExportGeoJsonAsync(id);
            return Results.Text(geoJson, "application/geo+json");
        });

        // Sector endpoints
        group.MapPost("/{searchAreaId:guid}/sectors", async (Guid searchAreaId, AddSectorRequest request, ISearchAreaService service) =>
        {
            var result = await service.AddSectorAsync(searchAreaId, request);
            return Results.Created($"/api/search-areas/sectors/{result.Id}", result);
        });

        group.MapGet("/sectors/{sectorId:guid}", async (Guid sectorId, ISearchAreaService service) =>
        {
            var result = await service.GetSectorByIdAsync(sectorId);
            return result.Any() ? Results.Ok(result.Single()) : Results.NotFound();
        });

        group.MapPut("/sectors/{sectorId:guid}/status", async (Guid sectorId, UpdateSectorStatusRequest request, ISearchAreaService service) =>
        {
            var result = await service.UpdateSectorStatusAsync(sectorId, request.Status);
            return Results.Ok(result);
        });

        group.MapPut("/sectors/{sectorId:guid}/assign", async (Guid sectorId, AssignSectorRequest request, ISearchAreaService service) =>
        {
            var result = await service.AssignSectorAsync(sectorId, request.UserId);
            return Results.Ok(result);
        });

        group.MapDelete("/sectors/{sectorId:guid}", async (Guid sectorId, ISearchAreaService service) =>
        {
            await service.DeleteSectorAsync(sectorId);
            return Results.NoContent();
        });

        return group;
    }
}
