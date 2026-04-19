namespace Timtek.SAR.Tests.Application.TeamManagement;

// FR-4.4.1 — Team creation and assignment
[Subject("Team Management")]
class When_a_case_manager_creates_a_search_team
{
    It should_create_the_team;
    It should_associate_the_team_with_the_case;
}

[Subject("Team Management")]
class When_assigning_a_team_lead_to_a_team
{
    It should_add_the_member_with_the_team_lead_role;
}

[Subject("Team Management")]
class When_assigning_a_drone_operator_to_a_team
{
    It should_add_the_member_with_the_drone_operator_role;
}

[Subject("Team Management")]
class When_assigning_a_ground_searcher_to_a_team
{
    It should_add_the_member_with_the_ground_searcher_role;
}

// FR-4.4.2 — Availability
[Subject("Team Management")]
class When_viewing_team_member_status
{
    It should_display_member_availability;
    It should_display_current_assignment_status;
}

// FR-4.4.3 — Equipment tracking
[Subject("Team Management")]
class When_tracking_resources_for_a_case
{
    It should_track_drones;
    It should_track_batteries;
    It should_track_radios;
    It should_track_vehicles;
}

// FR-4.4.4 — Search session scheduling
[Subject("Team Management")]
class When_scheduling_a_search_session
{
    It should_set_the_start_time;
    It should_set_the_end_time;
    It should_assign_sectors_to_the_session;
}

// FR-4.4.5 — Self-add (in-scope, meets threshold)
[Subject("Team Management")]
class When_an_in_scope_searcher_with_sufficient_reputation_self_adds_to_a_case
{
    It should_add_the_searcher_to_the_team_immediately;
    It should_not_require_case_manager_approval;
}

// FR-4.4.6 — Join request (out-of-scope or below threshold)
[Subject("Team Management")]
class When_an_out_of_scope_searcher_requests_to_join_a_case
{
    It should_create_a_pending_join_request;
    It should_require_case_manager_approval;
}

[Subject("Team Management")]
class When_a_searcher_below_reputation_threshold_requests_to_join_a_case
{
    It should_create_a_pending_join_request;
    It should_require_case_manager_approval;
}

[Subject("Team Management")]
class When_a_case_manager_approves_a_join_request
{
    It should_add_the_searcher_to_the_team;
}

[Subject("Team Management")]
class When_a_case_manager_rejects_a_join_request
{
    It should_not_add_the_searcher_to_the_team;
    It should_notify_the_searcher_of_the_rejection;
}

// FR-4.4.7 — In-scope determination
[Subject("Team Management")]
class When_determining_if_a_searcher_is_in_scope_for_a_case
{
    It should_be_in_scope_if_the_initial_report_location_falls_within_the_searchers_ao;
    It should_be_out_of_scope_if_the_initial_report_location_falls_outside_the_searchers_ao;
}
