namespace Timtek.SAR.Tests.Application.SearchAreaManagement;

// FR-4.3.1 — Defining search areas
[Subject("Search Area Management")]
class When_a_case_manager_defines_a_search_area_by_polygon
{
    It should_create_the_search_area_on_the_map;
    It should_store_the_polygon_geometry;
}

[Subject("Search Area Management")]
class When_a_case_manager_defines_a_search_area_by_circle
{
    It should_create_the_search_area_with_centre_and_radius;
}

[Subject("Search Area Management")]
class When_a_case_manager_defines_a_search_area_by_grid_overlay
{
    It should_create_the_search_area_as_a_grid;
}

[Subject("Search Area Management")]
class When_a_team_lead_defines_a_search_area
{
    It should_allow_the_team_lead_to_create_a_search_area;
}

// FR-4.3.2 — Sectors
[Subject("Search Area Management")]
class When_dividing_a_search_area_into_sectors
{
    It should_create_individual_sectors;
    It should_allow_assigning_sectors_to_teams;
    It should_allow_assigning_sectors_to_individual_searchers;
}

// FR-4.3.3 — Sector status tracking
[Subject("Search Area Management")]
class When_a_sector_is_created
{
    It should_set_the_status_to_not_started;
}

[Subject("Search Area Management")]
class When_a_sector_search_begins
{
    It should_change_status_to_in_progress;
}

[Subject("Search Area Management")]
class When_a_sector_search_is_completed
{
    It should_change_status_to_completed;
}

[Subject("Search Area Management")]
class When_a_sector_needs_re_searching
{
    It should_change_status_to_needs_re_search;
}

// FR-4.3.5 — Import/export
[Subject("Search Area Management")]
class When_exporting_a_search_area
{
    It should_support_geojson_format;
    It should_support_kml_format;
}

[Subject("Search Area Management")]
class When_importing_a_search_area
{
    It should_accept_geojson_format;
    It should_accept_kml_format;
}

// FR-4.3.6 — What3Words input
[Subject("Search Area Management")]
class When_a_what3words_address_is_provided_as_location_input
{
    It should_convert_the_w3w_address_to_gps_coordinates;
}

// FR-4.3.7 — Dual coordinate display
[Subject("Search Area Management")]
class When_displaying_a_geographic_coordinate
{
    It should_show_the_gps_latitude_and_longitude;
    It should_show_the_corresponding_what3words_address;
}

// FR-4.3.8 — W3W prefix
[Subject("Search Area Management")]
class When_displaying_a_what3words_address
{
    It should_include_the_triple_slash_prefix;
}
