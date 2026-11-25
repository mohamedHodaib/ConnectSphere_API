# ConnectSphere Backend — Repository Guide

## Solution Layout

The active solution file is `ConnectSphere/ConnectSphere.sln` (.NET 9.0, `Nullable` and `ImplicitUsings` enabled):

- **`ConnectSphere/`** (`ConnectSphere.API.csproj`) — ASP.NET Core Web API composition root, controllers, middlewares, exception handlers, auth setup, `Program.cs`, and `appsettings*.json`.
- **`ConnectSphere.Application/`** (`ConnectSphere.Application.csproj`) — CQRS commands/queries, MediatR handlers, pipeline behaviors, FluentValidation validators, AutoMapper profiles, DTOs, `Result`/`Error` types, pagination helpers, and repository/service interfaces.
- **`ConnectSphere.Ifrastructure/`** (`ConnectSphere.Infrastructure.csproj`) — EF Core `ApplicationDbContext` (SQL Server + Identity), entity configurations, migrations, repositories, `UnitOfWork`, infrastructure services, and database seeders. *(Note the folder spelling `ConnectSphere.Ifrastructure` referenced by `ConnectSphere.sln`.)*
- **`ConnectSphere.Domain/`** (`ConnectSphere.Domain.csproj`) — Domain entities and enums (`User` extends `IdentityUser<Guid>`).

> **Note on non-solution folders on disk:** `ConnectSphere.Infrastructure/`, `ConnectSphere.Models/`, `ConnectSphere.Tests/`, and `Project1/` exist at the repository root but are **not** included in `ConnectSphere/ConnectSphere.sln`.

## Architecture Rules

- **Domain depends on no other project** in the solution (only `Microsoft.Extensions.Identity.Stores` for `IdentityUser<Guid>`).
- **Application references only Domain** — it must never reference Infrastructure or API. All persistence and external concerns are abstracted behind interfaces (`IUnitOfWork`, `IRepositoryBase<T>`, `IAuthService`, `IEmailService`, `ITagService`, `IRoleService`, `ISaveFile`, `IUrlService`).
- **Infrastructure references Application** (and indirectly Domain) to implement DbContext, repositories, and services.
- **API is the composition root**: `Program.cs` wires `AddApplication()`, `AddIdentityAndAuthentication()`, `AddInfrastructure()`, exception handlers, CORS, middleware, and controllers.

## Observed Conventions

- **CQRS with MediatR (v13.1.0):**
  - Features live under `ConnectSphere.Application/Features/<Feature>/(Commands|Queries)/<UseCase>/`.
  - Requests are `public sealed record` implementing `IRequest<Result<T>>` or `IRequest<Result>` (with `MediatR.Unit` used for `Result<Unit>` commands).
  - Handlers are `internal sealed class` implementing `IRequestHandler<TRequest, TResponse>`.
  - Validators are `public class` inheriting `AbstractValidator<TRequest>` (and DTO validators where applicable), discovered via `AddValidatorsFromAssembly`.
  - Pipeline behaviors registered in `AddApplication`: `ValidationBehavior<,>` (throws `FluentValidation.ValidationException` on failure) and `CachingBehavior<,>` (active for requests implementing `ICacheable`).
- **Result & Error Handling:**
  - Business/domain failures return `Result.Failure(Error)` or `Result<T>.Failure(Error)` rather than throwing exceptions.
  - `Error` uses factory methods: `NotFound`, `Validation`, `Conflict`, `Unauthorized`, `Forbidden`, `General`.
  - Controllers inherit `ApiControllerBase` and return responses via `HandleResult`, `HandleCreatedResult`, or `HandlePagedResult` (which writes `X-Pagination` header and returns `Items`).
  - Unhandled `FluentValidation.ValidationException` is converted to HTTP 400 `ProblemDetails` by `FluentValidationExceptionHandler`; other unhandled exceptions are handled by `GlobalExceptionHandler` (HTTP 500 `ProblemDetails`).
- **Persistence:**
  - EF Core 9 with SQL Server (`ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>`).
  - Entity configurations use `IEntityTypeConfiguration<T>` in `ConnectSphere.Ifrastructure/Configurations/` and are applied via `ApplyConfigurationsFromAssembly`.
  - Repositories inherit `RepositoryBase<TEntity>` and are exposed lazily through `IUnitOfWork` / `UnitOfWork` (`CompleteAsync()` calls `SaveChangesAsync()`).
  - Pagination uses `PagedList<T>.ToPagedList(...)` and `PagedResult<T>` with `PaginationParameters` (`PageNumber` default `1`, `PageSize` default `10`, capped at `50`).
- **Authentication & Authorization:**
  - JWT Bearer authentication + ASP.NET Core Identity.
  - `AuthService` emits `JwtRegisteredClaimNames.Sub` with `user.Id.ToString()`, which inbound claim mapping surfaces as `ClaimTypes.NameIdentifier`. `JwtBearerEvents.OnTokenValidated` verifies `ClaimTypes.NameIdentifier` is a valid `Guid`.
  - Controllers read the authenticated user ID with `User.FindFirstValue(ClaimTypes.NameIdentifier)!`.
- **Logging & HTTP Pipeline:**
  - Serilog (Console, File, Seq via config) + `CorrelationIdMiddleware` (`X-Correlation-ID`) + `RequestLoggingMiddleware`.
  - `[assembly: ApiController]` is applied globally in `Program.cs`, with `JsonStringEnumConverter` for enum serialization.
  - `ConnectSphere.API.Program` is `public`, supporting `WebApplicationFactory<Program>`.

## Build & Test Commands

```powershell
# Restore and build the active solution
dotnet build ConnectSphere/ConnectSphere.sln

# Build without restore
dotnet build ConnectSphere/ConnectSphere.sln --no-restore

# Run tests (once test projects are added to the solution)
dotnet test ConnectSphere/ConnectSphere.sln
```

## Testing Conventions

- **Test framework:** xUnit
- **Mocking:** NSubstitute
- **Assertions:** Shouldly
- **Validator testing:** `FluentValidation.TestHelper` (`TestValidate`, `ShouldHaveValidationErrorFor`, `ShouldNotHaveValidationErrorFor`)
- **Database tests:** Use **SQLite in-memory (`Microsoft.Data.Sqlite`)** — **never** use the EF Core InMemory provider (`Microsoft.EntityFrameworkCore.InMemory`), and **do not** rely on Docker / Testcontainers. Keep a single open `SqliteConnection("DataSource=:memory:")` per test/fixture and call `EnsureCreatedAsync()` (avoid running SQL Server migrations against SQLite).
- **Visibility / Integration Test Notes:**
  - Application handlers (and most Infrastructure repositories) are `internal`; unit tests targeting them directly require `[assembly: InternalsVisibleTo("...")]` or testing through `IMediator`.
  - `WebApplicationFactory<Program>` tests should replace `ApplicationDbContext` with SQLite in-memory, supply `Jwt:Key` in test configuration, and disable/override `DatabaseInitializerHostedService` so SQL Server `MigrateAsync()` does not run against SQLite.
