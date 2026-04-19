namespace Timtek.SAR.Tests.Application.CaseManagement;

// FR-4.1.1 — Case creation
[Subject("Case Management")]
class When_a_case_manager_creates_a_case_from_a_lost_animal_report
{
    It should_create_a_new_case;
    It should_set_the_case_status_to_reported;
    It should_generate_a_unique_case_reference_number;
    It should_record_the_creation_in_the_activity_log;
}

[Subject("Case Management")]
class When_a_case_manager_creates_a_case_manually
{
    It should_create_a_new_case;
    It should_set_the_case_status_to_reported;
    It should_generate_a_unique_case_reference_number;
}

// FR-4.1.2 — Case details
[Subject("Case Management")]
class When_creating_a_case_with_animal_details
{
    It should_capture_the_species;
    It should_capture_the_breed;
    It should_capture_the_colour;
    It should_capture_the_size;
    It should_capture_distinguishing_features;
    It should_capture_a_photo;
    It should_capture_the_last_known_location;
    It should_capture_the_date_and_time_last_seen;
    It should_capture_owner_contact_details;
    It should_capture_medical_or_behavioural_notes;
}

// FR-4.1.3 — Case status progression
[Subject("Case Management")]
class When_triaging_a_reported_case
{
    It should_change_status_from_reported_to_triaged;
    It should_record_the_status_change_in_the_activity_log;
}

[Subject("Case Management")]
class When_starting_an_active_search_on_a_triaged_case
{
    It should_change_status_from_triaged_to_active_search;
    It should_record_the_status_change_in_the_activity_log;
}

[Subject("Case Management")]
class When_suspending_an_active_search
{
    It should_change_status_from_active_search_to_suspended;
    It should_record_the_status_change_in_the_activity_log;
}

[Subject("Case Management")]
class When_resolving_a_case
{
    It should_change_status_to_resolved;
    It should_record_the_status_change_in_the_activity_log;
    It should_require_an_outcome;
}

// FR-4.1.4 — Case outcomes
[Subject("Case Management")]
class When_resolving_a_case_as_reunited
{
    It should_set_the_outcome_to_reunited;
}

[Subject("Case Management")]
class When_resolving_a_case_as_recovered
{
    It should_set_the_outcome_to_recovered;
}

[Subject("Case Management")]
class When_resolving_a_case_as_rainbow_bridge
{
    It should_set_the_outcome_to_rainbow_bridge;
}

[Subject("Case Management")]
class When_resolving_a_case_as_cold_case
{
    It should_set_the_outcome_to_cold_case;
}

// FR-4.1.5 — Case priority
[Subject("Case Management")]
class When_assigning_a_priority_to_a_case
{
    It should_accept_critical_priority;
    It should_accept_high_priority;
    It should_accept_medium_priority;
    It should_accept_low_priority;
    It should_record_the_priority_change_in_the_activity_log;
}

// FR-4.1.6 — Case attachments
[Subject("Case Management")]
class When_attaching_a_file_to_a_case
{
    It should_accept_photo_attachments;
    It should_accept_document_attachments;
    It should_accept_drone_imagery_attachments;
    It should_record_the_attachment_in_the_activity_log;
}

// FR-4.1.7 — Activity log
[Subject("Case Management")]
class When_any_case_event_occurs
{
    It should_create_a_timestamped_activity_log_entry;
    It should_record_status_changes;
    It should_record_assignments;
    It should_record_notes;
    It should_record_sightings;
}
