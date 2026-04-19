namespace Timtek.SAR.Tests.Application.GroundSearchOperations;

// FR-4.8.1 — Search session logging
[Subject("Ground Search")]
class When_a_ground_searcher_logs_a_search_session
{
    It should_record_the_start_time;
    It should_record_the_end_time;
    It should_record_the_route_as_gps_track;
    It should_record_areas_covered;
}

// FR-4.8.2 — GPS track upload
[Subject("Ground Search")]
class When_a_ground_searcher_uploads_a_gps_track
{
    It should_accept_gpx_format;
    It should_display_the_route_on_the_case_map;
}

// FR-4.8.3 — Sighting reports
[Subject("Ground Search")]
class When_a_ground_searcher_reports_a_sighting
{
    It should_capture_the_description;
    It should_capture_a_photo;
    It should_capture_the_gps_location;
    It should_capture_the_timestamp;
}

// FR-4.8.4 — Environmental observations
[Subject("Ground Search")]
class When_a_ground_searcher_logs_an_environmental_observation
{
    It should_accept_terrain_conditions;
    It should_accept_hazards;
    It should_accept_animal_tracks;
    It should_accept_food_sources;
}
