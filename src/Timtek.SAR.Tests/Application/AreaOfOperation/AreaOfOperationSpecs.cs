namespace Timtek.SAR.Tests.Application.AreaOfOperation;

// FR-4.5.1 — Default AO on account creation
[Subject("Area of Operation")]
class When_a_new_searcher_account_is_created
{
    It should_apply_the_organisation_default_ao_radius;
    It should_centre_the_ao_on_the_searchers_home_address;
}

// FR-4.5.2 — AO definition levels
[Subject("Area of Operation")]
class When_a_searcher_defines_their_ao_at_base_level
{
    It should_store_the_home_address;
    It should_store_the_configurable_radius;
}

[Subject("Area of Operation")]
class When_a_searcher_defines_their_ao_at_postcode_level
{
    It should_accept_postcode_districts;
    It should_accept_postcode_sectors;
}

[Subject("Area of Operation")]
class When_a_searcher_defines_their_ao_at_polygon_level
{
    It should_accept_one_polygon;
    It should_accept_multiple_polygons;
    It should_store_arbitrary_geographic_regions;
}

// FR-4.5.3 — Combined AO
[Subject("Area of Operation")]
class When_a_searcher_has_multiple_ao_definition_levels
{
    It should_compute_the_effective_ao_as_the_union_of_all_defined_regions;
}

// FR-4.5.4 — AO visualisation
[Subject("Area of Operation")]
class When_a_searcher_views_their_effective_ao
{
    It should_display_the_combined_ao_on_an_interactive_map;
}

// FR-4.5.5 — AO editing
[Subject("Area of Operation")]
class When_a_searcher_edits_their_ao
{
    It should_allow_editing_at_any_time;
    It should_update_the_effective_ao;
}

// FR-4.5.6 — Manager/admin visibility
[Subject("Area of Operation")]
class When_a_case_manager_views_a_searchers_ao
{
    It should_display_the_searchers_effective_ao;
}

[Subject("Area of Operation")]
class When_an_administrator_views_a_searchers_ao
{
    It should_display_the_searchers_effective_ao;
}

// FR-4.5.7 — Independent editing
[Subject("Area of Operation")]
class When_editing_the_ao_radius
{
    It should_not_affect_the_postcode_definitions;
    It should_not_affect_the_polygon_definitions;
}

[Subject("Area of Operation")]
class When_editing_the_ao_postcode_list
{
    It should_not_affect_the_radius_definition;
    It should_not_affect_the_polygon_definitions;
}

[Subject("Area of Operation")]
class When_editing_the_ao_polygons
{
    It should_not_affect_the_radius_definition;
    It should_not_affect_the_postcode_definitions;
}
