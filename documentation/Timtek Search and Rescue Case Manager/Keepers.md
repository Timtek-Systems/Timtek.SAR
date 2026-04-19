Keepers are people who either own the lost animal or have some interest or duty of care to the animal. Keepers participate in a case without creating a user account; they are authenticated via secure, time-limited tokens.

> [!note] Design Decision: Security vs. Friction
> A cost-benefit analysis was performed on keeper authentication. Keeper access is scoped to a single case, exposes no searcher PII, and while keepers can post sightings and team messages, any misuse is visible to Case Managers and can be dealt with by revoking access. The most likely "abuse" of a forwarded invitation link is benign (sharing with another person who cares about the animal). Given the limited scope of access and the high friction cost of additional verification steps — particularly for people who are stressed about a lost pet — the decision was made to prioritise low friction over maximum security. Single-use, time-limited tokens with revoke-and-resend capability provide sufficient protection for this access level.

## Reporting Keeper

The person making the initial report shall be known as the "Reporting Keeper". This is typically the owner, but is often someone acting "pro procurationem", for example a foster keeper or someone looking after the pet while the keeper is away. The Reporting Keeper shall be required to declare their interest in the lost animal and make a declaration that they have the right to act on its behalf.

The Reporting Keeper shall automatically be a member of the case team. They shall have visibility of case status, search progress, sightings, team communications, and the case map. They shall be able to report sightings and post messages to the case team. They shall not have searcher-specific capabilities (reputation, AO, self-adding to other cases, uploading GPS tracks or flight logs).

The Reporting Keeper shall not be required to create a login. Access to case information shall be via a secure, time-limited, single-use authentication token sent to their verified contact details. Tokens shall be refreshed automatically when the keeper accesses the system via a notification link.

The Reporting Keeper shall be required at the time of the report to provide contact details including email and phone number. They shall verify their email address via a confirmation code before the report is submitted. They shall consent to receiving notifications via at least one of: email or WhatsApp (in-app push is not available to keepers as they have no user account).

If a second report is received for the same animal (identified by a Case Manager), Case Managers shall link the reports and determine the legitimate Reporting Keeper. The system shall not automatically grant Reporting Keeper status to duplicate reports.

## Associate Keepers

The Reporting Keeper may nominate Associate Keepers by providing their name and email or phone number. The system shall generate and send a unique invitation link to the nominated person via the selected channel.

Invitation links shall contain a secure, single-use token that expires after a configurable period (default: 7 days). The link may only be used once; after the first use it is consumed and cannot be reused. If the link is intercepted or used by the wrong person, the Reporting Keeper can revoke it and re-send a new invitation.

Upon clicking the link, the invitee shall be asked to confirm or update their contact details for their preferred notification channels. Upon confirmation, they become an Associate Keeper and are added to the case team with the same visibility as the Reporting Keeper.

Case Managers may also nominate Associate Keepers by the same mechanism.

## Post-Resolution: Searcher Conversion

Upon case resolution, all Keepers shall receive a case summary. The summary shall include an invitation to become a searcher for future cases, with a sign-up link. If the keeper clicks the link within a configurable period (default: 14 days), only their self-confirmed contact details shall be used to pre-populate a new searcher account registration. The keeper shall complete the standard registration process (including setting a password) before the account is created.

Upon account creation, the keeper's PII shall be removed from the keeper record and replaced with a reference to the new searcher account, preserving the case audit trail without retaining duplicate personal data.

## Data Retention and GDPR

Keepers who do not choose to become searchers within the configured period shall have their PII (name, email, phone number) automatically purged from the system. The case record and activity log shall be retained with keeper references anonymised (e.g., "Reporting Keeper", "Associate Keeper 1") to preserve the audit trail as required by FR-4.1.7.
