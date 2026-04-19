# Glossary

Defined terms and abbreviations used throughout the Timtek SAR Case Manager documentation.

AO
: **Area of Operation.** The geographic region(s) within which a searcher is prepared to respond to search cases. An AO may consist of one or more discontinuous regions defined by a radius, postcode districts/sectors, or map polygons. See [[Functional Specification#4.5 Searcher Area of Operation (AO)]].

Case
: A lost animal search operation managed within the system, from initial report through to resolution.

Case Outcome
: The final result recorded when a case is resolved. One of: **Reunited**, **Recovered**, **Rainbow Bridge**, or **Cold Case**. See [[Functional Specification#4.1 Case Management]].

Case Manager
: An authenticated role responsible for creating and managing cases, assigning teams, defining search areas, approving join requests, and overseeing operations. Any user with this role can manage any case within their organisation. See [[Roles#Case Manager]].

Cold Case
: A case outcome indicating the animal was not found within a reasonable time period.

Rainbow Bridge
: A case outcome indicating the animal was found deceased.

Recovered
: A case outcome indicating the animal was found alive but not reunited with the owner (e.g., owner went missing, refused to take the animal back, animal confiscated by authorities).

Reunited
: A case outcome indicating the animal was found alive and successfully reunited with the owner. This is the desired outcome for all cases.

Reputation Score
: A numeric value assigned to each searcher that reflects their participation and reliability. Reputation increases through positive activities (joining cases, confirmed sightings, uploading evidence) and decreases for inactivity when in scope. Searchers must meet a configurable reputation threshold to self-add to cases. See [[Functional Specification#4.6 Searcher Reputation]].

GPS
: **Global Positioning System.** Satellite-based navigation system used for location coordinates (latitude/longitude).

GPX
: **GPS Exchange Format.** An XML-based file format for storing GPS track data, waypoints, and routes.

GeoJSON
: An open standard format for encoding geographic data structures using JSON.

KML
: **Keyhole Markup Language.** An XML-based format for geographic visualisation, commonly used with Google Earth.

Keeper
: A person who owns the lost animal or has an interest or duty of care towards it. Keepers participate in a case without a user account, authenticated via secure, time-limited tokens. See [[Keepers]] and [[Roles#Keeper Roles]].

Reporting Keeper
: The keeper who made the initial lost animal report (or was confirmed as the legitimate reporter when duplicates are linked). Can view case progress, report sightings, post team messages, and nominate Associate Keepers. See [[Keepers#Reporting Keeper]].

Associate Keeper
: A keeper nominated by the Reporting Keeper or by Case Managers, with the same case-scoped visibility and posting capabilities as the Reporting Keeper. Cannot nominate further keepers. See [[Keepers#Associate Keepers]].

PII
: **Personally Identifiable Information.** Data that can identify a specific individual, such as name, address, phone number, or email.

SAR
: **Search and Rescue.** The coordinated effort to locate and assist lost or missing animals (in this context).

W3W
: **What3Words.** A geocoding system that encodes GPS coordinates as three-word addresses. W3W codes are always displayed with the `///` prefix (e.g., `///filled.count.soap`).
