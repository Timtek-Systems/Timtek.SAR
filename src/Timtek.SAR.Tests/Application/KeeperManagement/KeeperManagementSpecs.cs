namespace Timtek.SAR.Tests.Application.KeeperManagement;

// Reporting Keeper — creation and access
[Subject("Keeper Management")]
class When_a_lost_animal_report_is_submitted
{
    It should_designate_the_reporter_as_the_reporting_keeper;
    It should_require_declaration_of_interest_in_the_animal;
    It should_require_declaration_of_right_to_act;
    It should_add_the_reporting_keeper_to_the_case_team;
}

[Subject("Keeper Management")]
class When_a_reporting_keeper_accesses_the_case
{
    It should_allow_viewing_case_status;
    It should_allow_viewing_search_progress;
    It should_allow_viewing_sightings;
    It should_allow_viewing_team_communications;
    It should_allow_viewing_the_case_map;
    It should_allow_reporting_sightings;
    It should_allow_posting_messages_to_the_case_team;
    It should_not_allow_searcher_capabilities;
}

// Token-based authentication
[Subject("Keeper Management")]
class When_a_reporting_keeper_receives_an_authentication_token
{
    It should_be_time_limited;
    It should_be_single_use;
    It should_not_require_a_user_account;
}

[Subject("Keeper Management")]
class When_a_keeper_accesses_the_system_via_a_notification_link
{
    It should_refresh_the_authentication_token_automatically;
}

// Contact verification
[Subject("Keeper Management")]
class When_a_reporting_keeper_submits_their_contact_details
{
    It should_require_email_and_phone;
    It should_verify_the_email_via_confirmation_code;
    It should_require_consent_for_at_least_one_notification_channel;
}

// Duplicate reports
[Subject("Keeper Management")]
class When_a_case_manager_links_duplicate_reports
{
    It should_determine_the_legitimate_reporting_keeper;
    It should_not_automatically_grant_reporting_keeper_status_to_duplicates;
}

// Associate Keepers — invitation
[Subject("Keeper Management")]
class When_a_reporting_keeper_nominates_an_associate_keeper
{
    It should_generate_a_unique_invitation_link;
    It should_send_the_invitation_via_the_selected_channel;
}

[Subject("Keeper Management")]
class When_a_case_manager_nominates_an_associate_keeper
{
    It should_generate_a_unique_invitation_link;
    It should_send_the_invitation_via_the_selected_channel;
}

// Invitation token
[Subject("Keeper Management")]
class When_an_associate_keeper_invitation_link_is_generated
{
    It should_contain_a_secure_single_use_token;
    It should_expire_after_the_configurable_period;
}

[Subject("Keeper Management")]
class When_an_invitation_link_is_used
{
    It should_consume_the_token;
    It should_not_allow_reuse;
}

[Subject("Keeper Management")]
class When_a_reporting_keeper_revokes_an_invitation
{
    It should_invalidate_the_existing_token;
    It should_allow_sending_a_new_invitation;
}

// Associate Keeper onboarding
[Subject("Keeper Management")]
class When_an_invitee_clicks_the_associate_keeper_link
{
    It should_ask_to_confirm_or_update_contact_details;
    It should_ask_for_preferred_notification_channels;
}

[Subject("Keeper Management")]
class When_an_invitee_confirms_their_details
{
    It should_add_them_as_an_associate_keeper;
    It should_add_them_to_the_case_team;
    It should_grant_the_same_visibility_as_the_reporting_keeper;
}

// Post-resolution — searcher conversion
[Subject("Keeper Management")]
class When_a_case_is_resolved_and_keepers_receive_the_summary
{
    It should_include_an_invitation_to_become_a_searcher;
    It should_include_a_sign_up_link;
}

[Subject("Keeper Management")]
class When_a_keeper_clicks_the_searcher_sign_up_link_within_the_conversion_period
{
    It should_pre_populate_the_registration_from_self_confirmed_contact_details;
    It should_require_the_standard_registration_process;
    It should_require_setting_a_password;
}

[Subject("Keeper Management")]
class When_a_keeper_completes_searcher_registration
{
    It should_create_the_searcher_account;
    It should_remove_pii_from_the_keeper_record;
    It should_replace_pii_with_a_reference_to_the_new_searcher_account;
    It should_preserve_the_case_audit_trail;
}

// Data retention / GDPR
[Subject("Keeper Management")]
class When_the_conversion_period_expires_without_searcher_registration
{
    It should_purge_keeper_pii;
    It should_anonymise_keeper_references_in_the_case_record;
    It should_preserve_the_activity_log;
}
