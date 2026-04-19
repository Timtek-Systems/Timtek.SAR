# Copilot Repository Instructions — Timtek.SAR

This is an ASP.NET web application and API built with C#.

## Architecture & Project Structure

- Follow Clean Architecture: separate concerns into API (controllers/endpoints), Application (services/use cases), Domain (entities/value objects), and Infrastructure (data access/external services) layers.
- Use dependency injection for all service dependencies. Register services in `Program.cs` or dedicated extension methods.
- Prefer Minimal APIs for new endpoints unless the project already uses controllers consistently.
- Place shared DTOs and contracts in a dedicated contracts/models folder or project.

## Coding Standards

- **Keep it simple.** Only make changes that are directly requested or clearly necessary.
  - Do not add features, refactor code, or make improvements beyond what was asked.
  - Do not add XML doc comments, inline comments, or type annotations to code you did not change.
  - Do not add error handling for scenarios that cannot occur. Validate only at system boundaries (API endpoints, external inputs).
  - Do not create helper methods or abstractions for one-time operations.
- Use `var` when the type is obvious from the right-hand side; use explicit types otherwise.
- Prefer records for DTOs and immutable data. Use `sealed` on classes not designed for inheritance.
- Use C# collection expressions and primary constructors where the team has adopted them.
- Use `async`/`await` for all I/O-bound operations. Never block on async code (no `.Result`, `.Wait()`).
- Prefer `IActionResult` or `Results` return types on endpoints; use typed results (`Results<Ok<T>, NotFound>`) where practical.
- Follow the existing naming conventions in the codebase: `PascalCase` for public members, `_camelCase` for private fields.
- Read and understand existing code before modifying it. Respect established patterns in the project.

## Test-Driven Development

- **Write tests first.** For any new feature or bug fix:
  1. Write a failing test that defines the expected behavior.
  2. Implement the minimum code to make the test pass.
  3. Refactor while keeping tests green.
- Use **xUnit** as the test framework, **Moq** or **NSubstitute** for mocking, and **FluentAssertions** for readable assertions — unless the project already uses different libraries.
- Name tests using the pattern: `MethodName_Scenario_ExpectedResult` (e.g., `GetOrder_WhenNotFound_ReturnsNotFound`).
- One logical assertion per test. Multiple `Assert` calls are fine if they verify the same logical outcome.
- Organize tests to mirror the source project structure (e.g., `Timtek.SAR.Tests/Application/...`).
- Use `WebApplicationFactory<Program>` for integration tests against the API.
- Keep tests independent — no shared mutable state between tests.

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
