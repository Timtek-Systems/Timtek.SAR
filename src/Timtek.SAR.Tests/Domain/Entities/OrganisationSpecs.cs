using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Tests.Domain.Entities;

[Subject("Organisation")]
class When_creating_an_organisation
{
    static Organisation _result;

    Because of = () => _result = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };

    It should_store_the_name = () => _result.Name.ShouldEqual("Devon SAR");
    It should_have_an_id = () => _result.Id.ShouldNotEqual(Guid.Empty);
}

[Subject("Organisation")]
class When_an_organisation_is_created_with_default_settings
{
    static Organisation _result;

    Because of = () => _result = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };

    It should_have_a_default_ao_radius_of_25_km = () => _result.DefaultAoRadiusKm.ShouldEqual(25.0);
    It should_have_a_default_reputation_score_of_50 = () => _result.DefaultReputationScore.ShouldEqual(50);
    It should_have_a_minimum_reputation_threshold_of_10 = () => _result.MinimumReputationThreshold.ShouldEqual(10);
    It should_have_an_extended_notification_radius_of_50_km = () => _result.ExtendedNotificationRadiusKm.ShouldEqual(50.0);
    It should_have_a_keeper_invitation_expiry_of_7_days = () => _result.KeeperInvitationExpiryDays.ShouldEqual(7);
    It should_have_a_keeper_retention_period_of_365_days = () => _result.KeeperRetentionDays.ShouldEqual(365);
}

[Subject("Organisation")]
class When_an_organisation_has_default_reputation_point_values
{
    static Organisation _org;
    static ReputationPointValues _points;

    Establish context = () => _org = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };

    Because of = () => _points = _org.ReputationPointValues;

    It should_award_5_points_for_joining_a_case = () => _points.JoinCase.ShouldEqual(5);
    It should_award_20_points_for_found_outcome = () => _points.FoundOutcome.ShouldEqual(20);
    It should_award_10_points_for_confirmed_sighting = () => _points.ConfirmedSighting.ShouldEqual(10);
    It should_award_2_points_per_evidence_upload = () => _points.EvidenceUpload.ShouldEqual(2);
    It should_cap_evidence_uploads_at_10_per_case = () => _points.EvidenceUploadCapPerCase.ShouldEqual(10);
    It should_award_3_points_per_track_upload = () => _points.TrackUpload.ShouldEqual(3);
    It should_award_5_points_for_sector_completion = () => _points.SectorCompletion.ShouldEqual(5);
    It should_award_10_points_for_first_case_bonus = () => _points.FirstCaseBonus.ShouldEqual(10);
    It should_deduct_10_points_for_missed_in_scope_case = () => _points.MissedInScopeCase.ShouldEqual(-10);
    It should_deduct_15_points_for_removal_from_case = () => _points.RemovedFromCase.ShouldEqual(-15);
    It should_deduct_3_points_for_dismissed_sighting = () => _points.DismissedSighting.ShouldEqual(-3);
}

[Subject("Organisation")]
class When_updating_organisation_settings
{
    static Organisation _org;

    Establish context = () => _org = new Organisation { Id = Guid.NewGuid(), Name = "Devon SAR" };

    Because of = () =>
    {
        _org.DefaultAoRadiusKm = 30.0;
        _org.DefaultReputationScore = 100;
        _org.MinimumReputationThreshold = 20;
    };

    It should_store_the_updated_ao_radius = () => _org.DefaultAoRadiusKm.ShouldEqual(30.0);
    It should_store_the_updated_default_reputation = () => _org.DefaultReputationScore.ShouldEqual(100);
    It should_store_the_updated_minimum_threshold = () => _org.MinimumReputationThreshold.ShouldEqual(20);
}
