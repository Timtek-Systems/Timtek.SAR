using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;
using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Tests.Domain.Entities;

// --- Creation ---

[Subject("Case")]
class When_creating_a_case_with_all_details
{
    static Case _case;
    static Guid _organisationId;
    static Guid _createdBy;
    static GpsCoordinate _location;

    Establish context = () =>
    {
        _organisationId = Guid.NewGuid();
        _createdBy = Guid.NewGuid();
        _location = new GpsCoordinate(50.7184, -3.5339);
    };

    Because of = () => _case = new Case
    {
        Id = Guid.NewGuid(),
        OrganisationId = _organisationId,
        CreatedByUserId = _createdBy,
        AnimalSpecies = "Dog",
        AnimalBreed = "Border Collie",
        AnimalColour = "Black and white",
        AnimalSize = "Medium",
        AnimalDistinguishingFeatures = "White blaze on chest",
        AnimalPhotoUrl = "https://example.com/photo.jpg",
        LastKnownLatitude = _location.Latitude,
        LastKnownLongitude = _location.Longitude,
        DateTimeLastSeen = new DateTime(2026, 4, 19, 14, 30, 0, DateTimeKind.Utc),
        OwnerName = "Jane Smith",
        OwnerPhone = "07700900123",
        OwnerEmail = "jane@example.com",
        MedicalNotes = "Needs daily medication",
        BehaviouralNotes = "Nervous around strangers",
    };

    It should_have_reported_status = () => _case.Status.ShouldEqual(CaseStatus.Reported);
    It should_have_no_priority = () => _case.Priority.ShouldBeNull();
    It should_have_no_outcome = () => _case.Outcome.ShouldBeNull();
    It should_capture_the_species = () => _case.AnimalSpecies.ShouldEqual("Dog");
    It should_capture_the_breed = () => _case.AnimalBreed.ShouldEqual("Border Collie");
    It should_capture_the_colour = () => _case.AnimalColour.ShouldEqual("Black and white");
    It should_capture_the_size = () => _case.AnimalSize.ShouldEqual("Medium");
    It should_capture_distinguishing_features = () => _case.AnimalDistinguishingFeatures.ShouldEqual("White blaze on chest");
    It should_capture_the_photo_url = () => _case.AnimalPhotoUrl.ShouldEqual("https://example.com/photo.jpg");
    It should_capture_the_last_known_latitude = () => _case.LastKnownLatitude.ShouldEqual(50.7184);
    It should_capture_the_last_known_longitude = () => _case.LastKnownLongitude.ShouldEqual(-3.5339);
    It should_capture_the_date_time_last_seen = () => _case.DateTimeLastSeen.ShouldEqual(new DateTime(2026, 4, 19, 14, 30, 0, DateTimeKind.Utc));
    It should_capture_the_owner_name = () => _case.OwnerName.ShouldEqual("Jane Smith");
    It should_capture_the_owner_phone = () => _case.OwnerPhone.ShouldEqual("07700900123");
    It should_capture_the_owner_email = () => _case.OwnerEmail.ShouldEqual("jane@example.com");
    It should_capture_the_medical_notes = () => _case.MedicalNotes.ShouldEqual("Needs daily medication");
    It should_capture_the_behavioural_notes = () => _case.BehaviouralNotes.ShouldEqual("Nervous around strangers");
    It should_generate_a_reference_number = () => _case.ReferenceNumber.ShouldNotBeEmpty();
    It should_set_the_organisation = () => _case.OrganisationId.ShouldEqual(_organisationId);
    It should_set_the_creator = () => _case.CreatedByUserId.ShouldEqual(_createdBy);
}

// --- Status transitions ---

[Subject("Case")]
class When_triaging_a_reported_case
{
    static Case _case;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _case.Triage();

    It should_change_status_to_triaged = () => _case.Status.ShouldEqual(CaseStatus.Triaged);
}

[Subject("Case")]
class When_starting_active_search_on_a_triaged_case
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
    };

    Because of = () => _case.StartActiveSearch();

    It should_change_status_to_active_search = () => _case.Status.ShouldEqual(CaseStatus.ActiveSearch);
}

[Subject("Case")]
class When_suspending_an_active_search
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
        _case.StartActiveSearch();
    };

    Because of = () => _case.Suspend();

    It should_change_status_to_suspended = () => _case.Status.ShouldEqual(CaseStatus.Suspended);
}

[Subject("Case")]
class When_resuming_a_suspended_case
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
        _case.StartActiveSearch();
        _case.Suspend();
    };

    Because of = () => _case.StartActiveSearch();

    It should_change_status_to_active_search = () => _case.Status.ShouldEqual(CaseStatus.ActiveSearch);
}

[Subject("Case")]
class When_resolving_a_case_with_reunited_outcome
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
        _case.StartActiveSearch();
    };

    Because of = () => _case.Resolve(CaseOutcome.Reunited);

    It should_change_status_to_resolved = () => _case.Status.ShouldEqual(CaseStatus.Resolved);
    It should_set_the_outcome = () => _case.Outcome.ShouldEqual(CaseOutcome.Reunited);
}

[Subject("Case")]
class When_resolving_a_case_with_cold_case_outcome
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
        _case.StartActiveSearch();
        _case.Suspend();
    };

    Because of = () => _case.Resolve(CaseOutcome.ColdCase);

    It should_change_status_to_resolved = () => _case.Status.ShouldEqual(CaseStatus.Resolved);
    It should_set_the_outcome = () => _case.Outcome.ShouldEqual(CaseOutcome.ColdCase);
}

// --- Invalid transitions ---

[Subject("Case")]
class When_trying_to_start_active_search_on_a_reported_case
{
    static Case _case;
    static Exception _exception;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _exception = Catch.Exception(() => _case.StartActiveSearch());

    It should_throw_invalid_operation = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

[Subject("Case")]
class When_trying_to_triage_an_already_triaged_case
{
    static Case _case;
    static Exception _exception;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
    };

    Because of = () => _exception = Catch.Exception(() => _case.Triage());

    It should_throw_invalid_operation = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

[Subject("Case")]
class When_trying_to_resolve_a_reported_case
{
    static Case _case;
    static Exception _exception;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _exception = Catch.Exception(() => _case.Resolve(CaseOutcome.ColdCase));

    It should_throw_invalid_operation = () => _exception.ShouldBeOfExactType<InvalidOperationException>();
}

// --- Priority ---

[Subject("Case")]
class When_setting_case_priority
{
    static Case _case;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _case.SetPriority(CasePriority.Critical);

    It should_set_the_priority = () => _case.Priority.ShouldEqual(CasePriority.Critical);
}

[Subject("Case")]
class When_changing_case_priority
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.SetPriority(CasePriority.Low);
    };

    Because of = () => _case.SetPriority(CasePriority.High);

    It should_update_the_priority = () => _case.Priority.ShouldEqual(CasePriority.High);
}

static class CaseTestHelper
{
    public static Case CreateReportedCase() => new()
    {
        Id = Guid.NewGuid(),
        OrganisationId = Guid.NewGuid(),
        CreatedByUserId = Guid.NewGuid(),
        AnimalSpecies = "Cat",
        LastKnownLatitude = 51.5,
        LastKnownLongitude = -0.1,
        DateTimeLastSeen = DateTime.UtcNow,
        OwnerName = "Test Owner",
        OwnerEmail = "test@example.com",
    };
}
