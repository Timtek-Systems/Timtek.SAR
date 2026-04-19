# Functional Specification

## 1. Overview

The Timtek Search and Rescue (SAR) Case Manager is a multi-user web application that enables search and rescue teams to coordinate operations for finding lost pets and animals. Teams use a combination of drone operators and ground searchers to locate missing animals, and this system provides end-to-end case management from the initial report through to resolution.

## 2. Goals

- Provide a centralised platform for managing lost animal search cases.
- Coordinate drone operators and ground searchers across active search operations.
- Track search progress in real time with map-based visualisation.
- Maintain a complete audit trail for every case.
- Enable pet owners to report lost animals and receive status updates.

## 3. Users and Roles

| Role | Description |
|---|---|
| **Administrator** | Manages organisation settings, users, and system configuration. Full access to all cases and reports. |
| **Case Manager** | Creates and manages cases. Assigns teams, defines search areas, and oversees operations. |
| **Team Lead** | Coordinates a search team in the field. Updates search status and manages assigned searchers. |
| **Drone Operator** | Conducts aerial searches using drones. Logs flight paths, captures imagery, and reports sightings. |
| **Ground Searcher** | Conducts on-foot searches. Logs routes walked, areas covered, and reports sightings. |
| **Reporting Keeper** | The person who reported the lost animal. Token-based access scoped to their case; can view progress, report sightings, and message the team. |
| **Associate Keeper** | A person with interest or duty of care towards the lost animal, nominated by the Reporting Keeper or Case Managers. Same case-scoped access as Reporting Keeper. |

See [[Roles]] for detailed permissions and [[Keepers]] for keeper authentication and lifecycle.

## 4. Functional Requirements

### 4.1 Case Management

- **FR-4.1.1** The system shall allow Case Managers to create a new case from a lost animal report or manually.
- **FR-4.1.2** Each case shall capture: animal description (species, breed, colour, size, distinguishing features, photo), last known location, date/time last seen, owner contact details, and any medical or behavioural notes.
- **FR-4.1.3** Cases shall progress through defined statuses: Reported → Triaged → Active Search → Suspended → Resolved.
- **FR-4.1.4** A resolved case shall be assigned one of the following outcomes:
    - **Reunited** — The animal was found alive and reunited with the owner.
    - **Recovered** — The animal was found alive but not reunited with the owner.
    - **Rainbow Bridge** — The animal was found deceased.
    - **Cold Case** — The animal was not found within a reasonable time period.
- **FR-4.1.5** The system shall allow Case Managers to assign a priority level (Critical, High, Medium, Low) based on factors such as animal health, weather conditions, and terrain.
- **FR-4.1.6** The system shall support attaching files to a case: photos, documents, and drone imagery.
- **FR-4.1.7** The system shall maintain a timestamped activity log for each case recording all status changes, assignments, notes, and sightings.

### 4.2 Lost Animal Reporting (Public-Facing)

- **FR-4.2.1** The system shall provide a public-facing form for reporting a lost animal.
- **FR-4.2.2** The report form shall capture: reporter name, phone, email, animal details (species, breed, colour, size, distinguishing features), photo upload, last known location (address, map pin, or What3Words address), date/time last seen, and any additional notes.
- **FR-4.2.3** Upon submission the system shall generate a case reference number and send a confirmation to the reporter via email or SMS.
- **FR-4.2.4** Reporters shall be able to check the status of their case using the reference number without requiring a user account.

### 4.3 Search Area Management

- **FR-4.3.1** The system shall allow Case Managers and Team Leads to define search areas on an interactive map by drawing polygons, circles, or using a grid overlay.
- **FR-4.3.2** Search areas shall be divisible into sectors that can be individually assigned to teams or searchers.
- **FR-4.3.3** Each sector shall track its search status: Not Started, In Progress, Completed, Needs Re-search.
- **FR-4.3.4** The system shall display terrain data, satellite imagery, and relevant points of interest (water sources, shelters, roads) on the map.
- **FR-4.3.5** The system shall support importing and exporting search area definitions in standard geospatial formats (GeoJSON, KML).
- **FR-4.3.6** The system shall accept What3Words (W3W) addresses as location input wherever GPS coordinates are accepted, converting them to GPS coordinates via the W3W API.
- **FR-4.3.7** Wherever a geographic coordinate is displayed in the UI, the system shall show both the GPS coordinate (latitude/longitude) and the corresponding W3W address.
- **FR-4.3.8** W3W addresses shall always be displayed with the `///` prefix (e.g., `///filled.count.soap`).

### 4.4 Team and Resource Management

- **FR-4.4.1** The system shall allow Case Managers to create search teams and assign members by role (Team Lead, Drone Operator, Ground Searcher).
- **FR-4.4.2** The system shall display team member availability and current assignment status.
- **FR-4.4.3** The system shall allow tracking of equipment and resources allocated to a case (drones, batteries, radios, vehicles).
- **FR-4.4.4** The system shall support scheduling search sessions with defined start/end times and assigned sectors.
- **FR-4.4.5** Drone Operators and Ground Searchers whose Area of Operation (AO) encompasses the case's initial report location and who meet the organisation's minimum reputation threshold shall be able to add themselves to the search team for that case without Case Manager approval.
- **FR-4.4.6** Drone Operators and Ground Searchers whose AO does not encompass the case's initial report location, or who do not meet the minimum reputation threshold, shall be able to request to join the search team. Such requests must be approved by a Case Manager before the searcher is added.
- **FR-4.4.7** The system shall treat a searcher as "in scope" for any case whose initial report location falls within the searcher's defined AO.

### 4.5 Searcher Area of Operation (AO)

A searcher's Area of Operation defines the geographic region(s) within which they are prepared to respond to search cases. An AO may consist of one or more discontinuous regions.

- **FR-4.5.1** Each organisation shall define a default AO radius that is applied when a new searcher account is created, centred on the searcher's registered home address.
- **FR-4.5.2** Searchers shall be able to define their AO at three levels of complexity:
    1. **Base level** — Home address plus a configurable radius. This is the default, set automatically on account creation.
    2. **Postcode level** — A list of postcode districts and/or postcode sectors (e.g., "EX4", "EX1 1").
    3. **Polygon level** — One or more polygons drawn on an interactive map, defining arbitrary geographic regions.
- **FR-4.5.3** The three AO definition methods may be combined; the effective AO shall be the union of all defined regions.
- **FR-4.5.4** Searchers shall be able to view their effective AO on an interactive map.
- **FR-4.5.5** Searchers shall be able to edit their AO at any time from their user profile.
- **FR-4.5.6** Case Managers and Administrators shall be able to view any searcher's AO.
- **FR-4.5.7** The AO radius, postcode list, and polygon definitions shall be independently editable without affecting the other levels.

### 4.6 Searcher Reputation

The system maintains a reputation score for each searcher (Drone Operator or Ground Searcher) to incentivise active participation and recognise reliable contributors.

- **FR-4.6.1** Each searcher shall have a numeric reputation score that is publicly visible wherever their profile information is displayed.
- **FR-4.6.2** A searcher's reputation score shall never fall below zero.
- **FR-4.6.3** Each organisation shall configure a default reputation score assigned to new searcher accounts on creation.
- **FR-4.6.4** Each organisation shall configure a minimum reputation threshold required for a searcher to self-add to a case (see FR-4.4.5).
- **FR-4.6.5** The system shall automatically increase a searcher's reputation score for positive activities, including but not limited to:
    - Joining an active search case.
    - Being a member of a case where the animal is found (Reunited, Recovered, or Rainbow Bridge outcome).
    - Reporting a sighting that is subsequently confirmed.
    - Uploading images, video, GPS tracks, or other evidence.
- **FR-4.6.6** The system shall automatically decrease a searcher's reputation score when they are in scope for a case within their AO that concludes without them joining the search team.
- **FR-4.6.7** The reputation point values for each activity (gains and losses) shall be configurable per organisation. The following table defines the default values:

| Trigger | Points | Notes |
|---|---:|---|
| **Gains** | | |
| Join an active search case | +5 | Awarded once per case when the searcher joins the team |
| Case concludes with animal found (Reunited, Recovered, or Rainbow Bridge) | +20 | Awarded to all team members at case resolution |
| Report a sighting that is confirmed | +10 | Per confirmed sighting |
| Upload images or video evidence | +2 | Per upload, capped at +10 per case |
| Upload GPS track / flight log | +3 | Per track uploaded |
| Complete an assigned sector search | +5 | Sector status marked Completed by the searcher |
| First case joined (new searcher bonus) | +10 | One-time bonus on joining their very first case |
| **Losses** | | |
| In-scope case concludes without joining | −10 | Case was within AO and searcher did not join or request to join |
| Removed from case by Case Manager | −15 | Discretionary removal (not voluntary withdrawal) |
| Reported sighting marked as dismissed | −3 | Sighting reviewed and dismissed by a Case Manager |

- **FR-4.6.8** The system shall maintain a reputation history log per searcher, recording each change with the reason, point delta, and timestamp.

### 4.7 Drone Operations

- **FR-4.7.1** Drone Operators shall be able to log flight sessions including: drone identifier, start/end time, battery changes, flight path (GPS track), and altitude.
- **FR-4.7.2** The system shall accept uploaded flight path data (GPX or similar format) and display it on the case map.
- **FR-4.7.3** Drone Operators shall be able to upload imagery and video captured during flights, tagged with GPS coordinates and timestamps.
- **FR-4.7.4** The system shall support marking thermal or visual sightings from drone imagery with location and confidence level.
- **FR-4.7.5** The system shall track cumulative area coverage from drone flights per sector.

### 4.8 Ground Search Operations

- **FR-4.8.1** Ground Searchers shall be able to log search sessions including: start/end time, route taken (GPS track), and areas covered.
- **FR-4.8.2** The system shall accept GPS track uploads (GPX) and display routes on the case map.
- **FR-4.8.3** Ground Searchers shall be able to report sightings with: description, photo, GPS location, and timestamp.
- **FR-4.8.4** Ground Searchers shall be able to log environmental observations (terrain conditions, hazards, animal tracks, food sources).

### 4.9 Sightings and Evidence

- **FR-4.9.1** The system shall maintain a sightings log per case, recording all reported sightings from any source (drone, ground, public tip).
- **FR-4.9.2** Each sighting shall capture: source, GPS location, timestamp, description, photo/video, and a confidence rating (Confirmed, Probable, Possible, Unconfirmed).
- **FR-4.9.3** Sightings shall be displayed on the case map with visual indicators for confidence level.
- **FR-4.9.4** The system shall allow Case Managers to verify, dismiss, or flag sightings for follow-up.

### 4.10 Communication and Notifications

- **FR-4.10.1** The system shall provide an in-app messaging/notes capability per case for team coordination.
- **FR-4.10.2** The system shall support the following notification channels: email, WhatsApp, and in-app push notifications.
- **FR-4.10.3** Searchers shall be able to configure their preferred notification channels in their profile. WhatsApp notifications shall require the searcher to explicitly opt in and provide their phone number.
- **FR-4.10.4** When a new case is created, the system shall immediately notify all in-scope searchers (whose AO encompasses the case location) via their preferred notification channels. The notification shall include a direct action to join the search team in a single interaction (e.g., a button or link).
- **FR-4.10.5** Each organisation shall configure an extended notification radius beyond the case location. Searchers whose AO does not cover the case location but whose AO falls within this radius shall also be notified, with a clear indication that they are out of scope. The notification shall include a direct action to request to join the search team in a single interaction.
- **FR-4.10.6** All notifications that invite or enable a user action shall include a direct action element (button, link, or deep link) that performs that action immediately, without requiring the user to navigate to the relevant page manually.
- **FR-4.10.7** The system shall send notifications to team members for key events: status changes, new sightings, and case resolution.
- **FR-4.10.8** The system shall notify the Reporter when the case status changes or the animal is found.

### 4.11 Reporting and Analytics

- **FR-4.11.1** The system shall generate a case summary report including: timeline of events, search areas covered, total hours searched, sightings, and outcome.
- **FR-4.11.2** The system shall provide dashboard views showing: active cases, cases by status, cases by priority, team utilisation, and search coverage statistics.
- **FR-4.11.3** The system shall allow exporting reports in PDF and CSV formats.
- **FR-4.11.4** The system shall track organisation-level metrics: total cases, resolution rate, average time to resolution, and total search hours.

### 4.12 User Management and Authentication

- **FR-4.12.1** The system shall support user registration and authentication with email/password and optional multi-factor authentication.
- **FR-4.12.2** The system shall enforce role-based access control as defined in Section 3.
- **FR-4.12.3** Administrators shall be able to invite users, assign roles, and deactivate accounts.
- **FR-4.12.4** The system shall support multiple organisations (multi-tenancy), each with their own users, cases, and configuration.

## 5. Non-Functional Requirements

### 5.1 Performance

- **NFR-5.1.1** Pages and API responses shall load within 2 seconds under normal operating conditions.
- **NFR-5.1.2** The map view shall render search areas and tracks with no more than 500ms delay after data loads.
- **NFR-5.1.3** The system shall support at least 50 concurrent users per organisation without degradation.

### 5.2 Availability and Reliability

- **NFR-5.2.1** The system shall target 99.5% uptime during operational hours.
- **NFR-5.2.2** All data shall be backed up daily with point-in-time recovery capability.

### 5.3 Security

- **NFR-5.3.1** All communication shall be encrypted via HTTPS/TLS.
- **NFR-5.3.2** Passwords shall be hashed using a modern algorithm (e.g., bcrypt, Argon2).
- **NFR-5.3.3** The system shall comply with OWASP Top 10 security practices.
- **NFR-5.3.4** PII (reporter contact details) shall be stored encrypted at rest.
- **NFR-5.3.5** API endpoints shall be protected with authentication and authorisation checks.

### 5.4 Usability

- **NFR-5.4.1** The application shall be responsive and usable on mobile devices (field searchers will use phones/tablets).
- **NFR-5.4.2** The public reporting form shall be completable in under 5 minutes.
- **NFR-5.4.3** The system shall provide clear feedback for all user actions (success, error, loading states).
- **NFR-5.4.4** The system shall minimise user friction at every stage. Wherever a user is expected to take an action, the UI shall provide a direct element (button, link, or control) that performs that action with the minimum number of clicks or interactions.
- **NFR-5.4.5** Workflows shall be designed to avoid unnecessary navigation. Where possible, actions should be completable in context (e.g., inline approvals, one-tap joins) rather than requiring the user to navigate to a separate page.

### 5.5 Scalability

- **NFR-5.5.1** The architecture shall support horizontal scaling of the API tier.
- **NFR-5.5.2** File storage (imagery, GPS tracks) shall use cloud blob storage to scale independently.

### 5.6 Integration

- **NFR-5.6.1** The system shall expose a RESTful API for all core operations to enable third-party integration.
- **NFR-5.6.2** The map component shall integrate with a standard mapping provider (e.g., OpenStreetMap, Mapbox, Google Maps).
- **NFR-5.6.3** The system should support webhook notifications for case events.
- **NFR-5.6.4** The system shall integrate with the What3Words (W3W) API to convert between W3W addresses and GPS coordinates.

## 6. Data Model (Key Entities)

- **Organisation** — Tenant. Has users, cases, and configuration.
- **User** — Belongs to an organisation. Has a role.
- **AreaOfOperation** — A searcher's defined AO. Composed of a radius, postcode list, and/or polygon regions. Linked to a user.
- **Case** — A lost animal search operation. Linked to an organisation.
- **Animal** — Description of the lost animal. Linked to a case.
- **Reporter** — Contact who reported the lost animal. Linked to a case.
- **SearchArea** — A geographic area defined for a case, divided into sectors.
- **Sector** — A subdivision of a search area. Assigned to a team or individual.
- **SearchTeam** — A group of users assigned to a case.
- **SearchSession** — A logged search activity (flight or ground) within a sector.
- **FlightLog** — Drone flight details linked to a search session.
- **Sighting** — A reported observation of the animal or evidence.
- **Attachment** — A file (photo, video, document, GPS track) linked to a case, sighting, or session.
- **ReputationScore** — A searcher's current reputation value. Linked to a user.
- **ReputationEvent** — A timestamped log entry recording a reputation change (reason, point delta). Linked to a user.
- **ActivityLog** — Timestamped audit entry for a case.
- **Notification** — A message sent to a user or reporter.

## 7. Assumptions and Constraints

- Searchers will have intermittent mobile connectivity in the field; the system should handle offline scenarios gracefully where possible.
- Drone flight regulations vary by jurisdiction; the system does not enforce regulatory compliance but should capture relevant flight metadata.
- Initial deployment targets a single geographic region with English-language UI; internationalisation is deferred.
- GPS accuracy from consumer devices and drones is sufficient for search coordination but not survey-grade precision.

## 8. Future Considerations

The following items are out of scope for the initial release but are recognised as valuable future enhancements:

- **Social media integration** — Automated posting of case updates to Facebook groups (e.g., Drone SAR For Lost Dogs UK) or other social platforms via their APIs, enabling followers to receive real-time search progress without needing a system account.
- **Internationalisation** — Multi-language UI and locale-specific formatting.
