# Roles

The system uses role-based access control to determine what each user can see and do. Roles fall into two categories: **authenticated roles** (users with a login account belonging to an organisation) and **keeper roles** (case-scoped participants authenticated via token, with no user account).

A user may hold multiple authenticated roles within the same organisation. Permissions are additive — a user with both Case Manager and Drone Operator roles has the combined permissions of both.

## Authenticated Roles

### Administrator

Organisation-level system administrator responsible for configuration, user management, and oversight.

| Area | Permissions |
|---|---|
| Organisation settings | Full read/write access to all organisation configuration (defaults, thresholds, retention periods, notification settings) |
| User management | Invite users, assign and revoke roles, deactivate accounts |
| Cases | Full read access to all cases and reports within the organisation |
| Search areas | View all search areas and sectors |
| Searcher AOs | View any searcher's Area of Operation |
| Reputation | View all reputation scores and history; configure reputation point values |
| Reporting & analytics | Full access to dashboards, metrics, and exported reports |
| Notifications | Configure notification channels and templates |

Administrators do not typically participate in field operations but have visibility over all operational data for governance and support purposes.

### Case Manager

Operational coordinator who manages cases from report through to resolution. Any user with the Case Manager role can manage any case within their organisation — there is no concept of case ownership by a single person.

| Area | Permissions |
|---|---|
| Cases | Create, edit, prioritise, change status, and resolve cases (including assigning outcomes) |
| Search areas | Define and edit search areas and sectors on the case map |
| Teams | Create search teams, assign members by role, approve or reject join requests from out-of-scope searchers |
| Searcher removal | Remove a searcher from a case (triggers reputation penalty) |
| Sightings | Verify, dismiss, or flag sightings for follow-up |
| Keepers | Link duplicate reports and determine the legitimate Reporting Keeper; nominate Associate Keepers |
| Searcher AOs | View any searcher's Area of Operation |
| Attachments | Upload and view all case attachments |
| Activity log | Full read access to the case activity log |
| Reporting & analytics | Access to case summary reports, dashboards, and exports |

### Team Lead

Field coordinator who leads a search team during active operations. Team Leads are assigned to a specific case and team by a Case Manager.

| Area | Permissions |
|---|---|
| Search areas | Define and edit search areas and sectors for their assigned case |
| Team coordination | View team member availability and assignments; update search status |
| Search sessions | Create and manage scheduled search sessions with assigned sectors |
| Sightings | Report sightings; view all sightings on the case map |
| Team messaging | Post and read case team messages |
| Attachments | Upload and view attachments for their assigned case |
| Activity log | Read access to the activity log for their assigned case |

Team Leads do not have cross-case visibility or administrative capabilities.

### Drone Operator

Searcher who conducts aerial searches using drones. Drone Operators are members of the searcher pool and participate in cases based on their AO and reputation.

| Area | Permissions |
|---|---|
| Cases (in-scope) | View case details; self-add to the search team if AO covers the case location and reputation meets the threshold |
| Cases (out-of-scope) | View case summary; request to join (requires Case Manager approval) |
| AO | Define and edit their own Area of Operation (radius, postcode, polygon) |
| Flight logging | Log flight sessions (drone ID, times, battery changes, altitude, GPS track) |
| GPS tracks | Upload flight path data (GPX); view tracks on the case map |
| Imagery & video | Upload and tag drone imagery/video with GPS coordinates and timestamps |
| Thermal/visual sightings | Mark sightings from drone imagery with location and confidence level |
| Sightings | Report and view sightings |
| Team messaging | Post and read case team messages |
| Reputation | View own reputation score and history |
| Notifications | Configure preferred notification channels (email, WhatsApp, in-app push) |
| Profile | Edit own profile, contact details, and notification preferences |

### Ground Searcher

Searcher who conducts on-foot searches. Ground Searchers are members of the searcher pool and participate in cases based on their AO and reputation.

| Area | Permissions |
|---|---|
| Cases (in-scope) | View case details; self-add to the search team if AO covers the case location and reputation meets the threshold |
| Cases (out-of-scope) | View case summary; request to join (requires Case Manager approval) |
| AO | Define and edit their own Area of Operation (radius, postcode, polygon) |
| Search sessions | Log search sessions (times, route, areas covered) |
| GPS tracks | Upload route data (GPX); view tracks on the case map |
| Sightings | Report sightings with description, photo, GPS location, and timestamp |
| Environmental observations | Log terrain conditions, hazards, animal tracks, food sources |
| Team messaging | Post and read case team messages |
| Reputation | View own reputation score and history |
| Notifications | Configure preferred notification channels (email, WhatsApp, in-app push) |
| Profile | Edit own profile, contact details, and notification preferences |

## Keeper Roles

Keepers participate in a single case without creating a user account. They are authenticated via secure, time-limited tokens and have no access to any data outside their linked case. See [[Keepers]] for full details on authentication, invitation flows, and data retention.

### Reporting Keeper

The person who made the initial lost animal report, or who has been confirmed as the legitimate reporter when duplicate reports are linked.

| Area | Permissions |
|---|---|
| Case visibility | View case status, search progress, and the case map |
| Sightings | View all sightings; report new sightings |
| Team messages | View team communications; post messages to the case team |
| Associate Keepers | Nominate Associate Keepers by providing name and contact details; revoke and re-send invitation links |
| Notifications | Receive notifications for case status changes, new sightings, and case resolution via email or WhatsApp |

Reporting Keepers do **not** have: reputation scores, AO configuration, the ability to self-add to other cases, or the ability to upload GPS tracks or flight logs.

### Associate Keeper

A person nominated by the Reporting Keeper (or by Case Managers) who has an interest or duty of care towards the lost animal. Associate Keepers join the case via a single-use invitation link.

| Area | Permissions |
|---|---|
| Case visibility | View case status, search progress, and the case map |
| Sightings | View all sightings; report new sightings |
| Team messages | View team communications; post messages to the case team |
| Notifications | Receive notifications for case status changes, new sightings, and case resolution via email or WhatsApp |

Associate Keepers have the same visibility and posting capabilities as the Reporting Keeper but cannot nominate further Associate Keepers.
