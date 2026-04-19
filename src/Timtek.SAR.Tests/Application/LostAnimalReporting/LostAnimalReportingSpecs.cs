namespace Timtek.SAR.Tests.Application.LostAnimalReporting;

// FR-4.2.1 — Public-facing report form
[Subject("Lost Animal Reporting")]
class When_a_member_of_the_public_submits_a_lost_animal_report
{
    It should_create_a_new_case;
    It should_generate_a_case_reference_number;
    It should_send_a_confirmation_to_the_reporter;
}

// FR-4.2.2 — Report form fields
[Subject("Lost Animal Reporting")]
class When_submitting_a_lost_animal_report_with_all_details
{
    It should_capture_the_reporter_name;
    It should_capture_the_reporter_phone;
    It should_capture_the_reporter_email;
    It should_capture_the_animal_species;
    It should_capture_the_animal_breed;
    It should_capture_the_animal_colour;
    It should_capture_the_animal_size;
    It should_capture_distinguishing_features;
    It should_capture_a_photo_upload;
    It should_capture_the_last_known_location_as_address;
    It should_capture_the_last_known_location_as_map_pin;
    It should_capture_the_last_known_location_as_what3words;
    It should_capture_the_date_and_time_last_seen;
    It should_capture_additional_notes;
}

// FR-4.2.3 — Confirmation
[Subject("Lost Animal Reporting")]
class When_a_report_is_successfully_submitted
{
    It should_generate_a_case_reference_number;
    It should_send_confirmation_via_email;
    It should_send_confirmation_via_sms_if_phone_provided;
}

// FR-4.2.4 — Status check without account
[Subject("Lost Animal Reporting")]
class When_a_reporter_checks_case_status_with_a_reference_number
{
    It should_return_the_current_case_status;
    It should_not_require_a_user_account;
}

[Subject("Lost Animal Reporting")]
class When_a_reporter_checks_status_with_an_invalid_reference_number
{
    It should_not_reveal_whether_the_case_exists;
}
