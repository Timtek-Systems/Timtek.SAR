using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Domain.Entities;

[Subject("CaseActivityLog")]
class When_creating_an_activity_log_entry
{
    static CaseActivityLog _entry;
    static Guid _caseId;
    static Guid _userId;

    Establish context = () =>
    {
        _caseId = Guid.NewGuid();
        _userId = Guid.NewGuid();
    };

    Because of = () => _entry = new CaseActivityLog
    {
        Id = Guid.NewGuid(),
        CaseId = _caseId,
        UserId = _userId,
        ActivityType = CaseActivityType.StatusChanged,
        Description = "Status changed from Reported to Triaged",
    };

    It should_set_the_case_id = () => _entry.CaseId.ShouldEqual(_caseId);
    It should_set_the_user_id = () => _entry.UserId.ShouldEqual(_userId);
    It should_set_the_activity_type = () => _entry.ActivityType.ShouldEqual(CaseActivityType.StatusChanged);
    It should_set_the_description = () => _entry.Description.ShouldEqual("Status changed from Reported to Triaged");
    It should_have_a_timestamp = () => _entry.TimestampUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
}

[Subject("CaseActivityLog")]
class When_case_records_a_status_change
{
    static Case _case;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _case.Triage();

    It should_add_an_activity_log_entry = () => _case.ActivityLog.Count.ShouldEqual(1);
    It should_record_the_activity_type = () => _case.ActivityLog[0].ActivityType.ShouldEqual(CaseActivityType.StatusChanged);
    It should_include_the_old_status = () => _case.ActivityLog[0].Description.ShouldContain("Reported");
    It should_include_the_new_status = () => _case.ActivityLog[0].Description.ShouldContain("Triaged");
}

[Subject("CaseActivityLog")]
class When_case_records_a_priority_change
{
    static Case _case;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _case.SetPriority(CasePriority.High);

    It should_add_an_activity_log_entry = () => _case.ActivityLog.Count.ShouldEqual(1);
    It should_record_the_activity_type = () => _case.ActivityLog[0].ActivityType.ShouldEqual(CaseActivityType.PriorityChanged);
}

[Subject("CaseActivityLog")]
class When_case_records_an_outcome_on_resolve
{
    static Case _case;

    Establish context = () =>
    {
        _case = CaseTestHelper.CreateReportedCase();
        _case.Triage();
        _case.StartActiveSearch();
        _case.ActivityLog.Clear();
    };

    Because of = () => _case.Resolve(CaseOutcome.Reunited);

    It should_record_the_status_change = () => _case.ActivityLog.ShouldContain(e => e.ActivityType == CaseActivityType.StatusChanged);
    It should_record_the_outcome = () => _case.ActivityLog.ShouldContain(e => e.ActivityType == CaseActivityType.OutcomeAssigned);
}

[Subject("CaseActivityLog")]
class When_adding_a_note_to_a_case
{
    static Case _case;

    Establish context = () => _case = CaseTestHelper.CreateReportedCase();

    Because of = () => _case.AddNote("Possible sighting reported by member of public", Guid.NewGuid());

    It should_add_an_activity_log_entry = () => _case.ActivityLog.Count.ShouldEqual(1);
    It should_record_the_activity_type = () => _case.ActivityLog[0].ActivityType.ShouldEqual(CaseActivityType.NoteAdded);
    It should_include_the_note_text = () => _case.ActivityLog[0].Description.ShouldContain("Possible sighting");
}
