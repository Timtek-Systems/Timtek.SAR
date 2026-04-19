namespace Timtek.SAR.Tests.Application.DroneOperations;

// FR-4.7.1 — Flight session logging
[Subject("Drone Operations")]
class When_a_drone_operator_logs_a_flight_session
{
    It should_record_the_drone_identifier;
    It should_record_the_start_time;
    It should_record_the_end_time;
    It should_record_battery_changes;
    It should_record_the_flight_path_as_gps_track;
    It should_record_the_altitude;
}

// FR-4.7.2 — Flight path upload
[Subject("Drone Operations")]
class When_a_drone_operator_uploads_flight_path_data
{
    It should_accept_gpx_format;
    It should_display_the_flight_path_on_the_case_map;
}

// FR-4.7.3 — Imagery upload
[Subject("Drone Operations")]
class When_a_drone_operator_uploads_imagery
{
    It should_accept_photos;
    It should_accept_video;
    It should_tag_the_upload_with_gps_coordinates;
    It should_tag_the_upload_with_a_timestamp;
}

// FR-4.7.4 — Sightings from drone imagery
[Subject("Drone Operations")]
class When_a_drone_operator_marks_a_thermal_sighting
{
    It should_record_the_location;
    It should_record_the_confidence_level;
}

[Subject("Drone Operations")]
class When_a_drone_operator_marks_a_visual_sighting
{
    It should_record_the_location;
    It should_record_the_confidence_level;
}

// FR-4.7.5 — Area coverage tracking
[Subject("Drone Operations")]
class When_drone_flights_cover_a_sector
{
    It should_track_the_cumulative_area_coverage;
}
