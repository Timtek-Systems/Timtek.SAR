# Technology Stack

## Overview

Timtek.SAR is built on .NET 10 with a Blazor Web App front end, ASP.NET Minimal APIs, and PostgreSQL with PostGIS for spatial data. The architecture follows Clean Architecture principles with clearly separated layers.

## Runtime & Language

| Component | Technology |
|---|---|
| Language | C# 14 |
| Runtime | .NET 10 |
| Target Framework | `net10.0` |

## Front End

| Component | Technology | Purpose |
|---|---|---|
| UI Framework | Blazor Web App (Server + WebAssembly) | Interactive render mode with server-side pre-rendering and selective WASM for offline-capable components |
| CSS Framework | Bootstrap 5 | Responsive layout and UI components |
| Stylesheets | SCSS (Sass) | All CSS authored as `.scss` files; compiled to CSS at build time |
| Client-Side Script | TypeScript | All client-side code authored in `.ts` files; compiled to JavaScript at build time |
| Mapping | Leaflet.js / MapLibre GL JS | Interactive maps for sightings, search areas, drone tracks |
| Real-Time | SignalR | Live updates for active search cases, team coordination, sighting pins |

## Back End

| Component | Technology | Purpose |
|---|---|---|
| API | ASP.NET Minimal APIs | REST endpoints for mobile/external consumers |
| Authentication | ASP.NET Identity | User registration, login, role management |
| Authorization | Policy-based `[Authorize]` | Role and claim-based access control |
| Data Access Patterns | Timtek.Patterns.DataAccess | Repository, Unit of Work, and Query Specification patterns over EF Core |
| Background Jobs | Hangfire | Scheduled tasks (notifications, report generation, data cleanup) |
| Logging | `ILogger<T>` + Structured Logging | Request tracing with correlation IDs |

## Data Access

| Component | Technology | Purpose |
|---|---|---|
| Database | PostgreSQL 16+ | Primary relational data store |
| Spatial Extension | PostGIS | Geographic queries — sighting locations, search areas, geofencing |
| ORM | Entity Framework Core 10 | Data access, migrations, change tracking |
| Data Access Patterns | Timtek.Patterns.DataAccess | `IRepository<TEntity, TKey>`, `IUnitOfWork`, `QuerySpecification<T>` |
| Read Queries | `AsNoTracking()` | Performance optimization for read-only paths |

### Timtek.Patterns.DataAccess Conventions

- Access data only through `IRepository<TEntity, TKey>` and `IUnitOfWork` — never expose `DbContext` outside the infrastructure layer.
- All filtering and projection must be encapsulated in `QuerySpecification<T>` or `QuerySpecification<TIn, TOut>` classes — no ad-hoc `.Where()` calls in services.
- Single-entity lookups return `Maybe<T>` (from `TA.Utils.Core`), not `null`.
- Declare eager-load paths via `IFetchStrategy<T>` in the specification constructor — do not scatter `.Include()` calls across business logic.
- `IUnitOfWork` is the transaction boundary: call `CommitAsync()` to persist, or dispose to discard.
- All domain entities must implement `IDomainEntity<TKey>`.
- Use `MigrationChecker` at startup to halt if the database schema is out of date.

## Testing

| Component | Technology | Purpose |
|---|---|---|
| Test Framework | Machine.Specifications (MSpec) | BDD-style context/specification tests |
| Mocking | FakeItEasy | Fake dependencies in unit tests |
| Integration Tests | `WebApplicationFactory<Program>` | In-process API integration testing |
| Coverage Target | ≥ 90% | Enforced via coverlet |

## Infrastructure & DevOps

| Component | Technology | Purpose |
|---|---|---|
| Containerization | Docker + Docker Compose | Local development and deployment |
| CI/CD | GitHub Actions | Build, test, publish pipelines |
| Secrets | User Secrets (dev) / Azure Key Vault (prod) | Secure configuration management |
| Source Control | Git + GitHub | Repository hosting, project management, issue tracking |

## External Services

| Service | Purpose | Licensing |
|---|---|---|
| Map Tile Provider | Base map tiles (e.g., OpenStreetMap, Mapbox) | Free tier or paid depending on provider |
| what3words API | Location referencing using three-word addresses | Commercial API key required |

## Key Architectural Decisions

1. **Blazor over MVC** — Enables component reuse, real-time interactivity via SignalR, and selective WebAssembly for offline map features without maintaining a separate SPA framework.
2. **PostgreSQL + PostGIS over SQL Server** — Native spatial indexing and geographic functions at no licensing cost. Critical for sighting clustering, search area geometry, and proximity queries.
3. **Minimal APIs over Controllers** — Lighter-weight endpoint definitions with better alignment to vertical slice patterns. Controllers may be used where the project benefits from grouping.
4. **Timtek.Patterns.DataAccess over raw EF Core** — Enforces Repository + Unit of Work + Query Specification patterns. Reads are encapsulated in named specification classes; writes go through repository `Add`/`Remove` with explicit `IUnitOfWork.CommitAsync()`. This naturally separates query intent from mutation without needing a full CQRS/mediator framework.
5. **Hangfire over hosted services** — Dashboard visibility, retry policies, and cron scheduling for background work without custom infrastructure.
6. **MSpec + FakeItEasy** — BDD-style specifications with context-builder pattern for readable, maintainable tests aligned with team conventions.
7. **SCSS over plain CSS** — Variables, mixins, and nesting keep stylesheets maintainable. Never commit hand-written `.css` files; all styles must originate from `.scss` sources.
8. **TypeScript over JavaScript** — Static typing catches errors at compile time. Never commit hand-written `.js` files; all client-side code must originate from `.ts` sources.
9. **Thin Blazor components for testability** — Blazor components contain only UI binding and event wiring. All business logic, validation, data transformation, and orchestration lives in injectable service classes that can be unit tested with MSpec and FakeItEasy without rendering components. Code-behind (`.razor.cs`) files should delegate immediately to services; never put logic worth testing in a component.

## NuGet Package Management

Central Package Management (CPM) via `Directory.Packages.props`. Never add a `Version` attribute to `<PackageReference>` in individual project files — all versions are managed centrally.
