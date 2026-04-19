namespace Timtek.SAR.Tests.Application.SightingsAndEvidence;

// FR-4.9.1 — Sightings log
[Subject("Sightings")]
class When_a_sighting_is_reported_from_any_source
{
    It should_add_the_sighting_to_the_case_sightings_log;
}

[Subject("Sightings")]
class When_a_sighting_is_reported_by_a_drone_operator
{
    It should_record_the_source_as_drone;
}

[Subject("Sightings")]
class When_a_sighting_is_reported_by_a_ground_searcher
{
    It should_record_the_source_as_ground;
}

[Subject("Sightings")]
class When_a_sighting_is_reported_by_a_public_tip
{
    It should_record_the_source_as_public;
}

// FR-4.9.2 — Sighting details
[Subject("Sightings")]
class When_creating_a_sighting
{
    It should_capture_the_source;
    It should_capture_the_gps_location;
    It should_capture_the_timestamp;
    It should_capture_the_description;
    It should_capture_a_photo_or_video;
    It should_capture_the_confidence_rating;
}

[Subject("Sightings")]
class When_setting_a_sighting_confidence_rating
{
    It should_accept_confirmed;
    It should_accept_probable;
    It should_accept_possible;
    It should_accept_unconfirmed;
}

// FR-4.9.3 — Map display
[Subject("Sightings")]
class When_displaying_sightings_on_the_case_map
{
    It should_show_visual_indicators_for_each_confidence_level;
}

// FR-4.9.4 — Case manager review
[Subject("Sightings")]
class When_a_case_manager_verifies_a_sighting
{
    It should_mark_the_sighting_as_verified;
}

[Subject("Sightings")]
class When_a_case_manager_dismisses_a_sighting
{
    It should_mark_the_sighting_as_dismissed;
}

[Subject("Sightings")]
class When_a_case_manager_flags_a_sighting_for_follow_up
{
    It should_mark_the_sighting_as_flagged;
}
