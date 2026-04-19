using Timtek.SAR.Application.OrganisationManagement;
using Timtek.SAR.Application.OrganisationManagement.Dtos;

namespace Timtek.SAR.Api.Endpoints;

public static class OrganisationEndpoints
{
    public static RouteGroupBuilder MapOrganisationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/organisations");

        group.MapPost("/", async (CreateOrganisationRequest request, IOrganisationService service) =>
        {
            var organisation = await service.CreateAsync(request.Name);
            var response = MapToResponse(organisation);
            return Results.Created($"/api/organisations/{response.Id}", response);
        });

        group.MapGet("/{id:guid}", async (Guid id, IOrganisationService service) =>
        {
            var result = await service.GetAsync(id);
            return result.Any()
                ? Results.Ok(MapToResponse(result.Single()))
                : Results.NotFound();
        });

        group.MapPut("/{id:guid}/settings", async (Guid id, UpdateOrganisationSettingsRequest request, IOrganisationService service) =>
        {
            await service.UpdateSettingsAsync(id, org =>
            {
                org.DefaultAoRadiusKm = request.DefaultAoRadiusKm;
                org.DefaultReputationScore = request.DefaultReputationScore;
                org.MinimumReputationThreshold = request.MinimumReputationThreshold;
                org.ExtendedNotificationRadiusKm = request.ExtendedNotificationRadiusKm;
                org.KeeperInvitationExpiryDays = request.KeeperInvitationExpiryDays;
                org.KeeperRetentionDays = request.KeeperRetentionDays;
            });
            return Results.NoContent();
        });

        return group;
    }

    private static OrganisationResponse MapToResponse(Domain.Entities.Organisation org) => new(
        Id: org.Id,
        Name: org.Name,
        DefaultAoRadiusKm: org.DefaultAoRadiusKm,
        DefaultReputationScore: org.DefaultReputationScore,
        MinimumReputationThreshold: org.MinimumReputationThreshold,
        ExtendedNotificationRadiusKm: org.ExtendedNotificationRadiusKm,
        KeeperInvitationExpiryDays: org.KeeperInvitationExpiryDays,
        KeeperRetentionDays: org.KeeperRetentionDays,
        ReputationPointValues: new ReputationPointValuesResponse(
            JoinCase: org.ReputationPointValues.JoinCase,
            FoundOutcome: org.ReputationPointValues.FoundOutcome,
            ConfirmedSighting: org.ReputationPointValues.ConfirmedSighting,
            EvidenceUpload: org.ReputationPointValues.EvidenceUpload,
            EvidenceUploadCapPerCase: org.ReputationPointValues.EvidenceUploadCapPerCase,
            TrackUpload: org.ReputationPointValues.TrackUpload,
            SectorCompletion: org.ReputationPointValues.SectorCompletion,
            FirstCaseBonus: org.ReputationPointValues.FirstCaseBonus,
            MissedInScopeCase: org.ReputationPointValues.MissedInScopeCase,
            RemovedFromCase: org.ReputationPointValues.RemovedFromCase,
            DismissedSighting: org.ReputationPointValues.DismissedSighting));
}
