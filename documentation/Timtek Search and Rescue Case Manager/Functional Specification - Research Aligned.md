# Functional Specification (Research-Aligned)

_This document is a new specification and does not replace [[Functional Specification]]. It integrates recommendations from the research pack and explicitly marks differences from current implementation._

## 1. Scope and Intent

This specification defines the target behaviour for Timtek SAR after incorporating evidence-based recommendations from:

- [[research/Sanity Check Report]]
- [[research/Operational Models]]
- [[research/Workflow and UX Evidence]]
- [[research/Report for DSARLD]]

## 2. Implementation Delta Legend

| Marker | Meaning |
|---|---|
| **Implemented** | Present in the current codebase and API surface. |
| **Partial** | Some supporting pieces exist, but not full requirement behaviour. |
| **Not Implemented** | No production implementation found yet. |
| **Changed** | Behaviour differs from the existing implementation and must be updated. |

## 3. Functional Requirements

## 3.1 Foundation, Multi-Tenancy, Identity and Core Case Management

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.1.1 | The system shall support multi-tenant organisations with per-organisation settings. | **Implemented** |
| RAFR-3.1.2 | The system shall support registration, login, role assignment, role revocation, and user activation/deactivation. | **Implemented** |
| RAFR-3.1.3 | The system shall support case CRUD with lifecycle transitions (Reported, Triaged, Active Search, Suspended, Resolved), priority, outcome, and activity log. | **Implemented** |
| RAFR-3.1.4 | The system shall maintain a case reference number per case. | **Implemented** |
| RAFR-3.1.5 | The system shall support map-based search areas and sectors with sector status transitions and assignment. | **Implemented** |
| RAFR-3.1.6 | The system shall support GeoJSON export of search areas and sectors. | **Implemented** |
| RAFR-3.1.7 | The system shall support KML import/export for search areas. | **Not Implemented** |
| RAFR-3.1.8 | What3Words conversion services shall be available for location workflows. | **Partial** (service exists; end-to-end case/search workflows not complete) |

## 3.2 Keeper Safety and Public-Facing Workflow

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.2.1 | Public lost-animal reporting shall create a case without requiring account creation. | **Not Implemented** |
| RAFR-3.2.2 | Keeper access shall be token-based and case-scoped (Reporting Keeper + Associate Keepers). | **Not Implemented** |
| RAFR-3.2.3 | First keeper screen shall prominently present a safety checklist with "DO NOT CHASE" and bystander guidance scripts. | **Not Implemented** |
| RAFR-3.2.4 | Public case pages shall default to privacy-preserving outputs: masked contact details, reduced location precision, and indirect tip routing. | **Not Implemented** |
| RAFR-3.2.5 | Public photo handling shall support safeguarding defaults (face redaction or explicit consent path). | **Not Implemented** |
| RAFR-3.2.6 | Public forms shall be panic-state tolerant (single-column, low-friction, "I can't provide this now" path). | **Not Implemented** |

## 3.3 Area of Operation (AO), Searcher Capability, and Allocation Funnel

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.3.1 | Searchers shall define AO via radius, postcode, and polygons, with effective union used for in-scope determination. | **Not Implemented** |
| RAFR-3.3.2 | Searchers shall have explicit capability profiles (e.g., drone thermal-rated, scent dog handler, trapper) used in routing and assignment. | **Not Implemented** |
| RAFR-3.3.3 | Case dispatch shall capture and display the allocation funnel: in-scope -> notified -> acknowledged -> en route -> on scene -> contributing. | **Not Implemented** |
| RAFR-3.3.4 | Notifications shall support one-tap "can't make this one" responses to convert silence into signal. | **Not Implemented** |
| RAFR-3.3.5 | Notification system shall enforce per-searcher per-day alert caps. | **Not Implemented** |
| RAFR-3.3.6 | AO freshness shall be maintained through ambient confirmations during regular sign-in activity. | **Not Implemented** |

## 3.4 Reputation and Recognition (Research-Aligned Redesign)

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.4.1 | Reputation visibility shall be private-by-default (searcher + authorised coordinators/admins only). | **Changed** (existing design expects public score display) |
| RAFR-3.4.2 | The system shall not apply punitive negative reputation points for non-response; use neutral missed-response signals or gentle decay instead. | **Changed** (existing domain defaults include negative point values) |
| RAFR-3.4.3 | Reputation shall not be used as a hard gate for core participation without coordinator override. | **Changed** (existing settings include a minimum threshold model) |
| RAFR-3.4.4 | Recognition should prioritise outcome-tied badges/streaks over competitive leaderboards. | **Not Implemented** |
| RAFR-3.4.5 | Reputation/recognition behaviour shall be launch-configurable and measurable as an operational experiment. | **Partial** (org settings exist; redesigned mechanics absent) |

## 3.5 Case Intelligence: Temperament, Modality, and Theft Variant

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.5.1 | Pet temperament shall be a first-class structured field (e.g., skittish/social/aloof/xenophobic/hurt) and drive workflow guidance. | **Changed** (current case model stores behavioural notes as free text) |
| RAFR-3.5.2 | The system shall expose a recommended sequencing playbook based on temperament, elapsed time, and terrain. | **Not Implemented** |
| RAFR-3.5.3 | Case modality shall be explicit and transitionable (e.g., drone thermal, ground sweep, scent dog, trap-and-camera). | **Not Implemented** |
| RAFR-3.5.4 | Trap-and-camera shall be modelled as a dedicated multi-day deployment entity. | **Not Implemented** |
| RAFR-3.5.5 | The system shall warn on conflicting modality combinations (e.g., simultaneous drone + scent-dog in same sector). | **Not Implemented** |
| RAFR-3.5.6 | Theft cases shall be a workflow variant with dedicated fields and policy (crime reference, police liaison, altered public behaviour). | **Changed** (currently handled only as a case outcome/status concept) |

## 3.6 Pilot-Ground Handover, Audit, and Volunteer Cost Visibility

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.6.1 | Thermal contacts raised by pilots shall trigger nearest-capable ground responder alerts with one-tap response and ETA back to pilot. | **Not Implemented** |
| RAFR-3.6.2 | Deployment timeline shall be immutable for audit/insurance (assigned, acknowledged, start, end, verification events). | **Not Implemented** |
| RAFR-3.6.3 | The system shall produce one-click pilot deployment reports for insurer/compliance use. | **Not Implemented** |
| RAFR-3.6.4 | Per-case volunteer expense capture shall be supported (fuel/parking/consumables + receipt attachment). | **Not Implemented** |

## 3.7 Communications, Reporting, and Integration

| ID | Requirement | Status vs current implementation |
|---|---|---|
| RAFR-3.7.1 | Multi-channel notifications shall support email, WhatsApp (opt-in/template compliant), and in-app push. | **Not Implemented** |
| RAFR-3.7.2 | Event-driven notifications shall include direct-action links/buttons wherever user action is expected. | **Not Implemented** |
| RAFR-3.7.3 | Dashboarding shall include operational views and allocation funnel analytics. | **Not Implemented** |
| RAFR-3.7.4 | Case summary reporting shall include lifecycle timeline, search coverage, sightings, outcome, and exports. | **Not Implemented** |
| RAFR-3.7.5 | REST API and webhook surface shall cover core case events for integration. | **Partial** (core API exists; webhook layer absent) |

## 4. High-Impact Deltas from Current Implementation

1. **Reputation model must change** from public/penalty-driven to private/non-punitive recognition.
2. **Keeper safeguarding must be elevated** to first-class workflow (do-not-chase, privacy defaults, indirect contact).
3. **Behavioural intelligence must be structured** (temperament + playbook), not free-text only.
4. **Theft handling must become a dedicated workflow variant**, not only a status/outcome concept.
5. **Operational dispatch quality must be measurable** via explicit allocation funnel telemetry.
6. **Modality and handover orchestration must be explicit** (including trap deployments and pilot-ground ETA handoff).

## 5. Notes on Baseline Evidence for "Implemented"

Current "Implemented/Partial" markings are based on the existing codebase structure, including:

- API endpoints for organisations, auth/users, cases, and search areas/sectors
- Domain entities for organisation, case, activity log, search area, and sector
- Existing migrations covering these entities
- Infrastructure support for What3Words service registration/client
