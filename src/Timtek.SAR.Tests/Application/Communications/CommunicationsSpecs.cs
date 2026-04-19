namespace Timtek.SAR.Tests.Application.Communications;

// FR-4.10.1 — In-app messaging
[Subject("Communications")]
class When_a_team_member_posts_a_message_to_the_case
{
    It should_add_the_message_to_the_case_notes;
    It should_be_visible_to_all_case_team_members;
}

// FR-4.10.2 — Notification channels
[Subject("Communications")]
class When_sending_a_notification
{
    It should_support_email;
    It should_support_whatsapp;
    It should_support_in_app_push_notifications;
}

// FR-4.10.3 — Channel preferences
[Subject("Communications")]
class When_a_searcher_configures_notification_preferences
{
    It should_allow_selecting_preferred_channels;
    It should_require_explicit_opt_in_for_whatsapp;
    It should_require_a_phone_number_for_whatsapp;
}

// FR-4.10.4 — New case notification (in-scope searchers)
[Subject("Communications")]
class When_a_new_case_is_created
{
    It should_immediately_notify_all_in_scope_searchers;
    It should_use_each_searchers_preferred_notification_channels;
    It should_include_a_direct_action_to_join_the_search_team;
}

// FR-4.10.5 — Extended radius notification
[Subject("Communications")]
class When_a_new_case_is_created_and_searchers_are_within_extended_radius
{
    It should_notify_out_of_scope_searchers_within_the_extended_radius;
    It should_clearly_indicate_they_are_out_of_scope;
    It should_include_a_direct_action_to_request_to_join;
}

// FR-4.10.6 — Direct action elements
[Subject("Communications")]
class When_a_notification_invites_a_user_action
{
    It should_include_a_direct_action_element;
    It should_perform_the_action_immediately_when_clicked;
    It should_not_require_manual_navigation;
}

// FR-4.10.7 — Team event notifications
[Subject("Communications")]
class When_a_case_status_changes
{
    It should_notify_all_team_members;
}

[Subject("Communications")]
class When_a_new_sighting_is_reported
{
    It should_notify_all_team_members;
}

[Subject("Communications")]
class When_a_case_is_resolved
{
    It should_notify_all_team_members;
}

// FR-4.10.8 — Reporter notifications
[Subject("Communications")]
class When_the_case_status_changes_and_a_reporter_exists
{
    It should_notify_the_reporter;
}

[Subject("Communications")]
class When_the_animal_is_found
{
    It should_notify_the_reporter;
}
