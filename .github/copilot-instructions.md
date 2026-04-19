# Copilot Repository Instructions — Timtek.SAR

This is an ASP.NET web application and API built with C#.

## Architecture & Project Structure

- Follow Clean Architecture: separate concerns into API (controllers/endpoints), Application (services/use cases), Domain (entities/value objects), and Infrastructure (data access/external services) layers.
- Layered architecture: presentation → business logic → data access. Controllers and endpoints should not reference data access directly; go through service/logic layers.
- Use dependency injection for all service dependencies. Register services in `Program.cs` or dedicated extension methods.
- Prefer Minimal APIs for new endpoints unless the project already uses controllers consistently.
- Place shared DTOs and contracts in a dedicated contracts/models folder or project.

## Coding Standards

- **Keep it simple.** Only make changes that are directly requested or clearly necessary.
  - Do not add features, refactor code, or make improvements beyond what was asked.
  - Do not add XML doc comments, inline comments, or type annotations to code you did not change.
  - Do not add error handling for scenarios that cannot occur. Validate only at system boundaries (API endpoints, external inputs).
  - Do not create helper methods or abstractions for one-time operations.
- PascalCase for types and public members; `_camelCase` for private fields; `camelCase` for local variables.
- Guard clauses over nested `if` blocks.
- Avoid `out` parameters — prefer DTOs or wrapper types.
- Comments explain **why**, not what.
- Use `var` when the type is obvious from the right-hand side; use explicit types otherwise.
- Prefer records for DTOs and immutable data. Use `sealed` on classes not designed for inheritance.
- Use C# collection expressions and primary constructors where the team has adopted them.
- Use `async`/`await` for all I/O-bound operations. Never block on async code (no `.Result`, `.Wait()`).
- Prefer `IActionResult` or `Results` return types on endpoints; use typed results (`Results<Ok<T>, NotFound>`) where practical.
- Read and understand existing code before modifying it. Respect established patterns in the project.

## Blazor Component Design

- Keep Blazor components thin: UI binding and event wiring only.
- All business logic, validation, data transformation, and orchestration must live in injectable service classes, not in components or code-behind files.
- Code-behind (`.razor.cs`) files should delegate immediately to services — never put logic worth testing in a component.
- This maximises testability: services are unit tested with MSpec and FakeItEasy without needing to render components.

## Time

- Inject `TimeProvider` and call `timeProvider.GetUtcNow().UtcDateTime`. Never use `DateTime.UtcNow` directly.

## Test-Driven Development

- **Write tests first.** For any new feature or bug fix:
  1. Write a failing test that defines the expected behaviour.
  2. Implement the minimum code to make the test pass.
  3. Refactor while keeping tests green.
- Use **MSpec** (Machine.Specifications) as the test framework and **FakeItEasy** for mocking.
- Pattern: Context-Builder class + `Establish` / `Because` / `It` delegates.
- Class names fully encode the scenario — no separate description string needed.
- One assertion per `It` delegate.
- Shared setup lives in `*ContextBuilder.cs` files (one per domain area).
- Organize tests to mirror the source project structure (e.g., `Timtek.SAR.Tests/Application/...`).
- Use `WebApplicationFactory<Program>` for integration tests against the API.
- Keep tests independent — no shared mutable state between tests.
- Target at least 90% code coverage.

## Security (OWASP Top 10)

- Always use parameterized queries or EF Core — never concatenate user input into SQL.
- Validate and sanitize all external input at API boundaries. Use data annotations or FluentValidation.
- Use the `[Authorize]` attribute on endpoints that require authentication. Apply least-privilege policies.
- Never log secrets, tokens, passwords, or PII.
- Return generic error messages to clients; log detailed errors server-side only.
- Use HTTPS in all environments. Configure CORS to allow only trusted origins.

## Entity Framework & Data Access

- Use EF Core migrations for all schema changes.
- Keep `DbContext` lifetime scoped (the default for ASP.NET DI).
- Use `AsNoTracking()` for read-only queries.
- Avoid lazy loading; use explicit `.Include()` for related data.

## Error Handling & Logging

- Use middleware or exception filters for global error handling — not try/catch in every method.
- Use structured logging with `ILogger<T>`. Include correlation IDs for request tracing.
- Return appropriate HTTP status codes: 400 for bad input, 404 for missing resources, 500 only for unexpected failures.

## Configuration

- Use `appsettings.json` and environment-specific overrides. Bind to strongly-typed options classes with `IOptions<T>`.
- Never commit secrets. Use User Secrets in development and a vault (e.g., Azure Key Vault) in production.

## PowerShell

- Prefer PowerShell over other shells.
- Treat any PowerShell command over 100 characters or multi-line as long.
- Treat any command with more than one level of quoting/escaping as complex.
- Do not run complex commands directly at the command prompt; write a temporary script file instead to avoid blocking on input and to surface parse errors.

## Docker

- Docker persistent volumes are stored in a subdirectory called `persistent-volumes` under the `docker-compose.yaml` file location.
