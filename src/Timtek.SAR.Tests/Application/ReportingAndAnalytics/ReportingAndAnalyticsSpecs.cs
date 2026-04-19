namespace Timtek.SAR.Tests.Application.ReportingAndAnalytics;

// FR-4.11.1 — Case summary report
[Subject("Reporting")]
class When_generating_a_case_summary_report
{
    It should_include_the_timeline_of_events;
    It should_include_search_areas_covered;
    It should_include_total_hours_searched;
    It should_include_sightings;
    It should_include_the_outcome;
}

// FR-4.11.2 — Dashboard views
[Subject("Reporting")]
class When_viewing_the_dashboard
{
    It should_show_active_cases;
    It should_show_cases_by_status;
    It should_show_cases_by_priority;
    It should_show_team_utilisation;
    It should_show_search_coverage_statistics;
}

// FR-4.11.3 — Export
[Subject("Reporting")]
class When_exporting_a_report
{
    It should_support_pdf_format;
    It should_support_csv_format;
}

// FR-4.11.4 — Organisation metrics
[Subject("Reporting")]
class When_viewing_organisation_level_metrics
{
    It should_show_total_cases;
    It should_show_resolution_rate;
    It should_show_average_time_to_resolution;
    It should_show_total_search_hours;
}
