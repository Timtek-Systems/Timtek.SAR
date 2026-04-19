namespace Timtek.SAR.Tests.Application.CaseManagement;

// FR-4.1.6 — Case attachments (future work)
[Subject("Case Management")]
class When_attaching_a_file_to_a_case
{
    It should_accept_photo_attachments;
    It should_accept_document_attachments;
    It should_accept_drone_imagery_attachments;
    It should_record_the_attachment_in_the_activity_log;
}
