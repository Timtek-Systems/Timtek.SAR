namespace Timtek.SAR.Tests.Application.UserManagement;

// FR-4.12.1 — Registration and authentication
[Subject("User Management")]
class When_a_user_registers_with_email_and_password
{
    It should_create_the_user_account;
    It should_hash_the_password;
}

[Subject("User Management")]
class When_a_user_enables_multi_factor_authentication
{
    It should_require_mfa_on_subsequent_logins;
}

[Subject("User Management")]
class When_a_user_logs_in_with_valid_credentials
{
    It should_authenticate_the_user;
    It should_return_appropriate_claims;
}

[Subject("User Management")]
class When_a_user_logs_in_with_invalid_credentials
{
    It should_not_authenticate_the_user;
    It should_not_reveal_which_field_was_wrong;
}

// FR-4.12.2 — Role-based access control
[Subject("User Management")]
class When_a_user_has_the_administrator_role
{
    It should_grant_administrator_permissions;
}

[Subject("User Management")]
class When_a_user_has_the_case_manager_role
{
    It should_grant_case_manager_permissions;
}

[Subject("User Management")]
class When_a_user_has_the_team_lead_role
{
    It should_grant_team_lead_permissions;
}

[Subject("User Management")]
class When_a_user_has_the_drone_operator_role
{
    It should_grant_drone_operator_permissions;
}

[Subject("User Management")]
class When_a_user_has_the_ground_searcher_role
{
    It should_grant_ground_searcher_permissions;
}

[Subject("User Management")]
class When_a_user_has_multiple_roles
{
    It should_grant_the_combined_permissions_of_all_roles;
}

// FR-4.12.3 — User administration
[Subject("User Management")]
class When_an_administrator_invites_a_user
{
    It should_send_an_invitation;
}

[Subject("User Management")]
class When_an_administrator_assigns_a_role
{
    It should_update_the_users_roles;
}

[Subject("User Management")]
class When_an_administrator_deactivates_an_account
{
    It should_prevent_the_user_from_logging_in;
}

// FR-4.12.4 — Multi-tenancy
[Subject("User Management")]
class When_a_user_belongs_to_an_organisation
{
    It should_only_see_cases_within_their_organisation;
    It should_only_see_users_within_their_organisation;
    It should_only_see_configuration_for_their_organisation;
}
