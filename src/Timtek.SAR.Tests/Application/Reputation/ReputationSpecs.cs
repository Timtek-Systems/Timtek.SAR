namespace Timtek.SAR.Tests.Application.Reputation;

// FR-4.6.1 — Reputation visibility
[Subject("Reputation")]
class When_viewing_a_searchers_profile
{
    It should_display_the_reputation_score;
}

// FR-4.6.2 — Floor at zero
[Subject("Reputation")]
class When_a_reputation_loss_would_take_the_score_below_zero
{
    It should_set_the_score_to_zero;
}

// FR-4.6.3 — Default score
[Subject("Reputation")]
class When_a_new_searcher_account_is_created
{
    It should_assign_the_organisation_default_reputation_score;
}

// FR-4.6.4 — Threshold
[Subject("Reputation")]
class When_a_searcher_attempts_to_self_add_with_reputation_below_threshold
{
    It should_not_allow_self_adding;
    It should_require_a_join_request_instead;
}

// FR-4.6.5 — Reputation gains
[Subject("Reputation")]
class When_a_searcher_joins_an_active_search_case
{
    It should_increase_reputation_by_the_configured_join_points;
    It should_record_the_change_in_reputation_history;
}

[Subject("Reputation")]
class When_a_case_concludes_with_animal_found
{
    It should_award_found_points_to_all_team_members;
    It should_record_the_change_in_reputation_history;
}

[Subject("Reputation")]
class When_a_searcher_reports_a_sighting_that_is_confirmed
{
    It should_increase_reputation_by_the_configured_confirmed_sighting_points;
    It should_record_the_change_in_reputation_history;
}

[Subject("Reputation")]
class When_a_searcher_uploads_images_or_video
{
    It should_increase_reputation_by_the_configured_upload_points;
    It should_cap_upload_gains_at_the_per_case_maximum;
}

[Subject("Reputation")]
class When_a_searcher_uploads_a_gps_track_or_flight_log
{
    It should_increase_reputation_by_the_configured_track_upload_points;
}

[Subject("Reputation")]
class When_a_searcher_completes_an_assigned_sector
{
    It should_increase_reputation_by_the_configured_sector_completion_points;
}

[Subject("Reputation")]
class When_a_searcher_joins_their_very_first_case
{
    It should_award_the_new_searcher_bonus;
    It should_only_award_the_bonus_once;
}

// FR-4.6.6 — Reputation losses
[Subject("Reputation")]
class When_an_in_scope_case_concludes_without_the_searcher_joining
{
    It should_decrease_reputation_by_the_configured_missed_case_points;
    It should_record_the_change_in_reputation_history;
}

[Subject("Reputation")]
class When_a_case_manager_removes_a_searcher_from_a_case
{
    It should_decrease_reputation_by_the_configured_removal_points;
}

[Subject("Reputation")]
class When_a_reported_sighting_is_dismissed
{
    It should_decrease_reputation_by_the_configured_dismissed_sighting_points;
}

// FR-4.6.7 — Configurable point values
[Subject("Reputation")]
class When_an_organisation_configures_custom_reputation_point_values
{
    It should_use_the_custom_values_for_gains;
    It should_use_the_custom_values_for_losses;
}

// FR-4.6.8 — Reputation history
[Subject("Reputation")]
class When_a_reputation_change_occurs
{
    It should_record_the_reason;
    It should_record_the_point_delta;
    It should_record_the_timestamp;
}
