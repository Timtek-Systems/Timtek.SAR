# Implementation Plan

This document organises the Timtek SAR Case Manager into major functional blocks, each of which can be handed off to a developer (or pair) to work on as a self-contained unit. Blocks are ordered so that earlier blocks produce the foundations later blocks depend on.

---

## Dependency Graph (simplified)

```
Block 1  Foundation & Cross-Cutting
   ↓
Block 2  Multi-Tenancy & Organisation Settings
   ↓
Block 3  Identity, Roles & User Management
   ↓
Block 4  Case Management Core ──────────────────────┐
   ↓                                                 │
Block 5  Mapping & Geospatial Platform               │
   ↓                                                 │
Block 6  Search Area & Sector Management              │
   ↓                                                 │
Block 7  Area of Operation (AO)                       │
   ↓                                                 │
Block 8  Searcher Reputation ─────────────────────────┤
   ↓                                                 │
Block 9  Team & Resource Management                   │
   ↓                                                 │
Block 10  Drone Operations                            │
Block 11  Ground Search Operations                    │
   ↓                                                 │
Block 12  Sightings & Evidence                        │
   ↓                                                 │
Block 13  Keeper Management (Public Reporting + Tokens)│
   ↓                                                 │
Block 14  Communications & Notifications ─────────────┘
   ↓
Block 15  Reporting, Analytics & Dashboard
   ↓
Block 16  API Surface & Integration Points
   ↓
Block 17  Security Hardening & GDPR Compliance
   ↓
Block 18  Performance, Mobile UX & Polish
```

---

## Block 1 — Foundation & Cross-Cutting Infrastructure

**Goal:** Establish the running skeleton that all other blocks build on.

| Layer | Work |
|---|---|
| Domain | Base entity interface (`IDomainEntity<TKey>`), common value objects (GPS coordinate, W3W address) |
| Infrastructure | `SarDbContext` with PostgreSQL + PostGIS, EF Core migration pipeline, `IRepository` / `IUnitOfWork` / `QuerySpecification` wiring via Timtek.Patterns.DataAccess, `MigrationChecker` at startup |
| **EF Core Data Model** | Design the initial data model covering all domain entities identified across Blocks 2–14. Create `IEntityTypeConfiguration<T>` classes for the Block 1–2 entities (`Organisation`). Establish conventions for table naming, key generation, index strategy, relationship mapping, and value-object conversion. Each subsequent block adds its own configurations but the conventions and base wiring are set here. |
| API | Remove weather stub; configure DI, HTTPS, OpenAPI, global error-handling middleware, structured logging with `ILogger<T>` + correlation IDs |
| Web | Blazor Web App shell (layout, navigation), Bootstrap 5 + SCSS build pipeline, TypeScript build pipeline |
| DevOps | Docker Compose for local PostgreSQL + PostGIS, GitHub Actions CI (build + test) |

**Backlog items:** TSAR-1, TSAR-65

**Exit criteria:** `dotnet build` succeeds, migrations run against a local database, health-check endpoint responds, CI pipeline is green, MSpec test runner executes.

### EF Core Data Model — Incremental Delivery

The full EF Core data model is built incrementally. Each block is responsible for:

1. Creating `IEntityTypeConfiguration<T>` classes for its entities in the Infrastructure layer.
2. Registering configurations in `SarDbContext` via `ApplyConfigurationsFromAssembly`.
3. Generating an EF Core migration for the schema changes introduced by the block.
4. Writing integration tests that verify the entity can be round-tripped through the database.

| Block | Entity Configurations Added |
|---|---|
| 1 | `SarDbContext` scaffolding, conventions, `Organisation` |
| 3 | `User`, `Role` (via ASP.NET Identity) |
| 4 | `Case`, `CaseActivityLog` |
| 6 | `SearchArea`, `Sector` (PostGIS geometry columns) |
| 7 | `AreaOfOperation` (PostGIS geometry columns) |
| 8 | `ReputationScore`, `ReputationHistoryEntry` |
| 9 | `SearchTeam`, `TeamMember`, `JoinRequest`, `Equipment`, `SearchSession` |
| 10 | `FlightSession`, `DroneImagery` |
| 11 | `GroundSearchSession`, `EnvironmentalObservation` |
| 12 | `Sighting` |
| 13 | `Keeper`, `KeeperToken`, `KeeperInvitation` |
| 14 | `CaseMessage` |

---

## Block 2 — Multi-Tenancy & Organisation Settings

**Goal:** Introduce the Organisation entity as the tenancy root and its configurable settings.

| Layer | Work |
|---|---|
| Domain | `Organisation` entity with settings: default AO radius, default reputation score, minimum reputation threshold, extended notification radius, keeper invitation expiry, keeper retention period, reputation point-value table |
| Application | Organisation CRUD service, strongly-typed `IOptions<T>` bindings |
| Infrastructure | Organisation EF mapping, seed data for development |
| API / Web | Admin UI and API for organisation settings |

**Backlog items:** TSAR-2, TSAR-6

**Depends on:** Block 1

---

## Block 3 — Identity, Roles & User Management

**Goal:** Users can register, log in, and be assigned roles. Admins can invite and manage users.

| Layer | Work |
|---|---|
| Domain | User entity, Role enum / entities (Administrator, Case Manager, Team Lead, Drone Operator, Ground Searcher) |
| Application | Registration, login, email verification, MFA, role assignment / revocation, user deactivation |
| Infrastructure | ASP.NET Identity integration, Argon2/bcrypt password hashing, JWT or cookie auth, email sending for verification & invitations |
| API / Web | Registration & login pages, admin user-management pages, `[Authorize]` policy enforcement |

**Backlog items:** TSAR-3, TSAR-4, TSAR-5, TSAR-63

**Depends on:** Block 2

---

## Block 4 — Case Management Core

**Goal:** Case Managers can create, edit, prioritise, and progress cases through their lifecycle. Activity log records all changes.

| Layer | Work |
|---|---|
| Domain | `Case` entity (animal description, last known location, status enum, priority enum, outcome enum), `CaseActivityLog` entity |
| Application | Case CRUD, status workflow engine (Reported → Triaged → Active Search → Suspended → Resolved), priority assignment, outcome assignment, activity-log recording |
| Infrastructure | Case EF mappings, file-attachment storage (cloud blob) |
| API / Web | Case list, case detail, case creation form, status/priority controls, activity log view, file upload |

**Backlog items:** TSAR-7, TSAR-8, TSAR-9, TSAR-10, TSAR-11

**Depends on:** Block 3

---

## Block 5 — Mapping & Geospatial Platform

**Goal:** Interactive map component available for all map-based features; What3Words integration operational.

| Layer | Work |
|---|---|
| Domain | Location value objects (GPS ↔ W3W), geospatial geometry types |
| Application | W3W conversion service (GPS ↔ W3W), map data DTOs |
| Infrastructure | W3W API client, PostGIS spatial query helpers |
| Web | Leaflet.js / MapLibre GL JS component (satellite imagery, terrain, POIs), reusable Blazor map wrapper, dual GPS + W3W display |

**Backlog items:** TSAR-19, TSAR-20

**Depends on:** Block 1

**Note:** Can be developed in parallel with Blocks 2–4 since it has no dependency on identity or case management. Later blocks integrate their features onto this map.

---

## Block 6 — Search Area & Sector Management

**Goal:** Case Managers and Team Leads define search areas on the map, divide them into sectors, and track sector status.

| Layer | Work |
|---|---|
| Domain | `SearchArea` entity (polygon, circle, grid geometry), `Sector` entity with status enum (Not Started, In Progress, Completed, Needs Re-search) |
| Application | Search area CRUD, sector CRUD, sector status transitions, GeoJSON/KML import and export |
| Infrastructure | Geometry persistence (PostGIS), GeoJSON/KML parser |
| Web | Drawing tools (polygon, circle, grid overlay) on case map, sector visual status overlay, import/export UI |

**Backlog items:** TSAR-21, TSAR-22, TSAR-23

**Depends on:** Block 4, Block 5

---

## Block 7 — Area of Operation (AO)

**Goal:** Searchers define their AO at three levels; system computes the effective AO as a union of all levels.

| Layer | Work |
|---|---|
| Domain | `AreaOfOperation` entity, AO level value objects (radius, postcode list, polygon set) |
| Application | AO CRUD per level, union computation, in-scope determination (does a point fall within a searcher's AO?), default AO creation on account setup |
| Infrastructure | Postcode → boundary resolution, PostGIS union and containment queries |
| Web | AO editor on searcher profile (radius slider, postcode entry, polygon drawing), AO visualisation map, admin view of any searcher's AO |

**Backlog items:** TSAR-29, TSAR-30, TSAR-31, TSAR-32

**Depends on:** Block 3, Block 5

---

## Block 8 — Searcher Reputation

**Goal:** Reputation scores are tracked, awarded, deducted, and displayed.

| Layer | Work |
|---|---|
| Domain | `ReputationScore` entity, `ReputationHistoryEntry` entity, reputation trigger definitions |
| Application | Reputation initialisation on account creation, gain triggers (join case, found outcome, confirmed sighting, upload evidence, complete sector, first-case bonus), loss triggers (missed in-scope case, removed from case, dismissed sighting), floor-at-zero enforcement |
| Infrastructure | Reputation persistence, configurable point-value lookup from organisation settings |
| Web | Reputation badge on searcher profiles, reputation history view |

**Backlog items:** TSAR-33, TSAR-34, TSAR-35, TSAR-36

**Depends on:** Block 2 (org settings), Block 3 (searcher accounts)

---

## Block 9 — Team & Resource Management

**Goal:** Case Managers build search teams, searchers self-join or request to join, and resources are tracked.

| Layer | Work |
|---|---|
| Domain | `SearchTeam` entity, `TeamMember` entity, `JoinRequest` entity, `Equipment` entity, `SearchSession` entity |
| Application | Team creation, member assignment by role, self-add (in-scope + reputation check), join-request workflow (request → approve/reject), equipment CRUD, search session scheduling |
| Infrastructure | Persistence, AO + reputation threshold query for eligibility check |
| Web | Team roster on case view, self-add / request-to-join buttons, join-request approval queue, equipment list, session scheduler |

**Backlog items:** TSAR-24, TSAR-25, TSAR-26, TSAR-27, TSAR-28

**Depends on:** Block 4, Block 7, Block 8

---

## Block 10 — Drone Operations

**Goal:** Drone Operators log flights, upload GPX tracks and imagery, mark sightings from imagery, and coverage is tracked.

| Layer | Work |
|---|---|
| Domain | `FlightSession` entity, `DroneImagery` entity |
| Application | Flight session CRUD, GPX parsing, imagery upload (GPS + timestamp tagged), thermal/visual sighting creation from imagery, cumulative area coverage calculation per sector |
| Infrastructure | GPX file parser, blob storage for imagery/video, coverage geometry calculations |
| Web | Flight log form, GPX upload, flight path on case map, imagery gallery with map pins, sighting-marking tool, sector coverage overlay |

**Backlog items:** TSAR-37, TSAR-38, TSAR-39, TSAR-40, TSAR-41

**Depends on:** Block 6, Block 9

**Note:** Can be developed in parallel with Block 11.

---

## Block 11 — Ground Search Operations

**Goal:** Ground Searchers log sessions, upload GPX tracks, report sightings, and log environmental observations.

| Layer | Work |
|---|---|
| Domain | `GroundSearchSession` entity, `EnvironmentalObservation` entity |
| Application | Session CRUD, GPX parsing, sighting creation, environmental observation logging |
| Infrastructure | GPX parser (shared with Block 10), blob storage |
| Web | Search session form, GPX upload, route on case map, sighting report form, observation log |

**Backlog items:** TSAR-42, TSAR-43, TSAR-44, TSAR-45

**Depends on:** Block 6, Block 9

**Note:** Can be developed in parallel with Block 10.

---

## Block 12 — Sightings & Evidence

**Goal:** Unified sightings log per case with confidence levels, map display, and case manager review workflow.

| Layer | Work |
|---|---|
| Domain | `Sighting` entity (source, location, timestamp, description, media, confidence rating) |
| Application | Sighting creation (from drone, ground, public tip, keeper), confidence assignment, case manager review (verify, dismiss, flag), reputation integration (trigger gain/loss on verify/dismiss) |
| Infrastructure | Sighting persistence, media blob storage |
| Web | Sightings log view, sightings on case map with confidence-level icons/colours, filtering by source and confidence, verify/dismiss/flag controls |

**Backlog items:** TSAR-46, TSAR-47, TSAR-48

**Depends on:** Block 4, Block 5, Block 8

**Note:** Sighting creation entry points in Blocks 10, 11, and 13 all feed into this shared sighting model.

---

## Block 13 — Keeper Management (Public Reporting & Token Auth)

**Goal:** Public lost-animal report form, Reporting Keeper token auth, Associate Keeper invitations, duplicate linking, post-resolution conversion, and GDPR purge.

| Layer | Work |
|---|---|
| Domain | `Keeper` entity (Reporting / Associate), `KeeperToken` entity, `KeeperInvitation` entity |
| Application | Public report form processing, case reference generation, email confirmation, token generation & refresh, Reporting Keeper case view, Associate Keeper invitation flow (nominate → send → confirm), duplicate report linking, post-resolution searcher conversion, GDPR data retention job |
| Infrastructure | Token generation & validation, email/SMS sending, Hangfire scheduled purge job, PII anonymisation |
| Web | Public report form (no login), case status check page (no login), keeper case view (status, map, sightings, messaging), associate nomination UI |

**Backlog items:** TSAR-12, TSAR-13, TSAR-14, TSAR-15, TSAR-16, TSAR-17, TSAR-18, TSAR-64

**Depends on:** Block 4

---

## Block 14 — Communications & Notifications

**Goal:** In-app messaging per case, multi-channel notifications (email, WhatsApp, push), preference management, and smart notification targeting.

| Layer | Work |
|---|---|
| Domain | `CaseMessage` entity, notification channel preferences, notification templates |
| Application | Case messaging service, notification dispatcher (channel routing by preference), new-case notification to in-scope searchers, extended-radius notification to nearby searchers, event-driven team/keeper notifications (status change, sighting, resolution), direct-action link generation |
| Infrastructure | Email provider integration, WhatsApp Business API client, SignalR hub for in-app push, Hangfire for async delivery, template rendering |
| Web | Case message thread UI, notification preference settings in user profile, notification centre (in-app) |

**Backlog items:** TSAR-49, TSAR-50, TSAR-51, TSAR-52, TSAR-53, TSAR-54, TSAR-55

**Depends on:** Block 4, Block 7 (AO for in-scope determination)

---

## Block 15 — Reporting, Analytics & Dashboard

**Goal:** Case summary reports, operational dashboards, organisation metrics, and export capabilities.

| Layer | Work |
|---|---|
| Application | Case summary report generation (timeline, areas, hours, sightings, outcome), dashboard data aggregation (active cases, by status, by priority, team utilisation, coverage stats), organisation metrics (total cases, resolution rate, average time to resolution, total search hours) |
| Infrastructure | Reporting queries (read-optimised), PDF generation, CSV export |
| Web | Dashboard page (Case Manager / Admin), case summary report view, export controls (PDF, CSV), organisation metrics page |

**Backlog items:** TSAR-56, TSAR-57, TSAR-58

**Depends on:** Block 4, Block 6, Block 9

---

## Block 16 — API Surface & Integration Points

**Goal:** RESTful API for all core operations with OpenAPI documentation, plus webhook support for external integrations.

| Layer | Work |
|---|---|
| API | Ensure every core operation is exposed via Minimal API endpoints with consistent conventions, proper HTTP status codes, and OpenAPI/Swagger documentation |
| Application | Webhook dispatch service (case creation, status change, resolution, new sighting), webhook registration and management |
| Infrastructure | Webhook delivery with retry-on-failure, JSON payload serialisation |

**Backlog items:** TSAR-59, TSAR-60

**Depends on:** Blocks 4–14 (exposes existing operations)

**Note:** API endpoints should be built incrementally alongside each block. This block captures the final audit, documentation pass, and webhook layer.

---

## Block 17 — Security Hardening & GDPR Compliance

**Goal:** PII encryption at rest, CORS lockdown, OWASP compliance audit, and full authorisation coverage.

| Layer | Work |
|---|---|
| Infrastructure | PII encryption via EF Core value converters, encryption key management (Azure Key Vault), CORS configuration for trusted origins only |
| API | Audit all endpoints for `[Authorize]`, input validation at API boundaries, parameterised queries verification, generic error responses |
| DevOps | Database backup and recovery configuration, restore procedure test |

**Backlog items:** TSAR-61, TSAR-66, TSAR-68

**Depends on:** All functional blocks

---

## Block 18 — Performance, Mobile UX & Polish

**Goal:** Responsive mobile UI, performance baselines, load testing, and UX polish.

| Layer | Work |
|---|---|
| Web | Responsive layout audit (phone + tablet), field-critical workflow testing (sighting reporting, case map, messaging, join actions on small screens) |
| Performance | Establish baselines (page/API < 2s, map render < 500ms), load test at 50 concurrent users, bottleneck resolution |
| UX | NFR-5.4.4 / NFR-5.4.5 audit — direct action elements everywhere, minimal navigation, inline approvals |

**Backlog items:** TSAR-62, TSAR-67

**Depends on:** All functional blocks

---

## Parallelism Opportunities

The following blocks can be developed concurrently by separate developers/pairs:

| Track | Blocks | Notes |
|---|---|---|
| **Core + Cases** | 1 → 2 → 3 → 4 | Sequential foundation chain |
| **Mapping** | 5 | Independent of identity/cases; start as soon as Block 1 is done |
| **AO + Reputation** | 7, 8 | Start after Block 3; independent of Block 4 |
| **Drone + Ground** | 10, 11 | Parallel with each other after Block 9 |
| **Keepers** | 13 | Start after Block 4; independent of Blocks 7–11 |
| **Comms** | 14 | Start after Block 4 + Block 7 |

This allows up to **4 developers** to work productively in parallel once Block 1 is complete, scaling to **5–6** once Blocks 3 and 4 are done.

---

## Backlog Item Cross-Reference

| Block | Backlog Items |
|---|---|
| 1 — Foundation | TSAR-1, TSAR-65 |
| 2 — Multi-Tenancy | TSAR-2, TSAR-6 |
| 3 — Identity & Roles | TSAR-3, TSAR-4, TSAR-5, TSAR-63 |
| 4 — Case Management | TSAR-7, TSAR-8, TSAR-9, TSAR-10, TSAR-11 |
| 5 — Mapping Platform | TSAR-19, TSAR-20 |
| 6 — Search Areas | TSAR-21, TSAR-22, TSAR-23 |
| 7 — Area of Operation | TSAR-29, TSAR-30, TSAR-31, TSAR-32 |
| 8 — Reputation | TSAR-33, TSAR-34, TSAR-35, TSAR-36 |
| 9 — Teams & Resources | TSAR-24, TSAR-25, TSAR-26, TSAR-27, TSAR-28 |
| 10 — Drone Ops | TSAR-37, TSAR-38, TSAR-39, TSAR-40, TSAR-41 |
| 11 — Ground Search | TSAR-42, TSAR-43, TSAR-44, TSAR-45 |
| 12 — Sightings | TSAR-46, TSAR-47, TSAR-48 |
| 13 — Keepers | TSAR-12, TSAR-13, TSAR-14, TSAR-15, TSAR-16, TSAR-17, TSAR-18, TSAR-64 |
| 14 — Communications | TSAR-49, TSAR-50, TSAR-51, TSAR-52, TSAR-53, TSAR-54, TSAR-55 |
| 15 — Reporting | TSAR-56, TSAR-57, TSAR-58 |
| 16 — API & Webhooks | TSAR-59, TSAR-60 |
| 17 — Security & GDPR | TSAR-61, TSAR-66, TSAR-68 |
| 18 — Performance & UX | TSAR-62, TSAR-67 |
