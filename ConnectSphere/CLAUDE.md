# ConnectSphere Backend

## Project Overview

ConnectSphere is a social media platform backend built with .NET 9.0 using a layered architecture. The project includes API, Application, Domain, and Infrastructure layers with ASP.NET Core Identity for authentication and Entity Framework Core for data access.

## Architecture

The project uses a standard **Layered Architecture**:
- **API Layer**: Web API controllers, middleware, and HTTP pipeline configuration  
- **Application Layer**: Business logic services and application features  
- **Domain Layer**: Core entities and domain rules (User, Post, Comment, Reaction, Follow, etc.)  
- **Infrastructure Layer**: Database context (ApplicationDbContext), identity configuration, and persistence logic

## Project Structure

```
ConnectSphere/
├── Controllers/                    # Web API controllers
├── Domain/
│   ├── Entities/                  # Core domain entities
│   └── Enums/                     # Enum types
├── Infrastructure/
│   ├── Identity/                 # ASP.NET Core Identity configuration
│   ├── Persistence/              # ApplicationDbContext and EF Core setup
│   ├── Configurations/            # EF Core entity configurations
│   └── Seed/                     # Database seeding
├── ConnectSphere.API.csproj        # API project with dependencies
├── ConnectSphere.Domain.csproj     # Domain project
├── ConnectSphere.Infrastructure.csproj  # Infrastructure project
├── Program.cs                     # Application startup and configuration
└── appsettings.json               # Configuration files
```

## Dependency Injection

The application follows standard ASP.NET Core dependency injection patterns:
- Program.cs registers services using extension methods: `AddApplication()`, `AddInfrastructure()`, `AddIdentityAndAuthentication()`  
- No custom DI containers or advanced patterns are used

## Authentication & Authorization

**ASP.NET Core Identity is used**:
- `ApplicationUser` extends `IdentityUser<Guid>`  
- Controllers use ASP.NET Core Identity mechanisms  
- CORS is configured with "AllowFrontend" policy for development

## Database & EF Core

**Entity Framework Core 9.0** is used for database access:
- `ApplicationDbContext` in Infrastructure/Persistence inherits from `IdentityDbContext<ApplicationUser>`  
- Entity mappings configured with `ApplyConfigurationsFromAssembly()`

## API & Controllers

The API follows RESTful conventions:
- Controllers located in Controllers/ directory (AuthController.cs, PostsController.cs, UsersController.cs, etc.)  
- Inherit from `ApiControllerBase` with `[ApiController]` attribute  
- Standard HTTP verbs (GET, POST, PUT, DELETE)

## DTOs and Mapping

AutoMapper is referenced in ConnectSphere.API.csproj but actual mapping implementations could not be verified.

## Validation

Validation implementation could not be verified — no specific validation files, attributes, or patterns were found in the verified files.

## Error Handling

Error handling implementation could not be verified — no specific exception handling middleware or patterns were found in the verified files.

## Logging

**Serilog is implemented**:
- Configured in Program.cs with multiple sinks (Console, File, configuration-based)  
- Uses daily rolling file output with structured logging format  
- Includes timestamp, level, message, and exception details

## OpenAPI

**OpenAPI 3.0** is implemented:
- `builder.Services.AddOpenApi()` registers OpenAPI services  
- `app.MapScalarApiReference()` provides Scalar UI for API documentation  
- Endpoints available in development only

## Naming Conventions

- **Classes**: PascalCase  
- **Methods/Properties**: PascalCase  
- **Files**: PascalCase (AuthController.cs, PostsController.cs)  
- **Namespaces**: Hierarchical matching project organization  
- **Local variables**: camelCase

## Development Rules

- Follow existing directory structure and naming conventions  
- Place code in the appropriate layer (API, Application, Domain, or Infrastructure)  
- Preserve existing Serilog logging patterns  
- Follow established controller and endpoint conventions  
- Follow the existing EF Core data-access patterns. Do not introduce a Repository Pattern unless explicitly requested.  
- Maintain existing ASP.NET Core Identity authentication mechanisms

## Things Claude MUST NOT Do

- Do not introduce new architectural patterns without explicit approval (Clean Architecture, CQRS, MediatR, Repository Pattern, Unit of Work, Specification Pattern, Result Pattern, etc.)  
- Do not replace existing implementations just because common patterns exist elsewhere  
- Do not create redundant code — reuse existing services and utilities  
- Do not modify unrelated system components when fixing issues  
- Do not assume package dependencies mean implementation — verify actual usage  
- Do not change the Layered Architecture established in the project  
- Do not modify core authentication mechanisms beyond existing ASP.NET Core Identity patterns  
- Do not restructure the existing project layout

## Known Issues / Inconsistencies

- **Solution Configuration Issue**: The ConnectSphere.sln file references `..\\ConnectSphere.Ifrastructure\\ConnectSphere.Infrastructure.csproj` while the actual project folder is named `ConnectSphere.Infrastructure`. This mismatch between the solution file and file system can cause build system confusion and must be verified before making Infrastructure-related changes.  
- **AutoMapper Unverified**: AutoMapper is listed as a dependency in ConnectSphere.API.csproj but usage was not verified. Verify actual implementation before using it.  
- **Missing Implementations**: Some expected patterns (FluentValidation, DTOs/mapping) are referenced as packages but actual implementation could not be verified.
