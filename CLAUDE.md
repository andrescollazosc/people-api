# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

`frydek` is a .NET 9 Web API that exposes CRUD operations for `Person` entities. The solution is structured following Clean Architecture, splitting concerns across four projects with strict dependency direction (outer layers depend on inner layers, never the reverse).

The repository layer is currently a **mock** — `GetById` and `GetAll` return hard-coded data, and `Create`/`Delete` are no-ops (`await Task.CompletedTask`). A real persistence backend has not been wired yet.

## Common commands

Run these from the solution root (`/frydek`):

```bash
# Restore + build the whole solution
dotnet build

# Run the API (Development environment, HTTPS on :7039, HTTP on :5171)
dotnet run --project Frydek.People.App

# Build a single project
dotnet build Frydek.People.App/Frydek.People.App.csproj

# Open Swagger UI (after `dotnet run`)
# http://localhost:5171/swagger
```

No test project exists yet in the solution.

## Architecture

Four projects, dependency direction flows inward: `App → Application → Core` and `Infrastructure → Application → Core`. Core has no dependencies.

- **`Frydek.People.Core`** — Domain layer. Entities (`Person`) and domain exceptions (`NotFoundException`). No framework references.
- **`Frydek.People.Application`** — Use cases, DTOs, and repository *interfaces* (`IPersonRepository`). Each use case is a single-purpose class with an `ExecuteAsync` method. Interfaces live directly under `UseCases/`; implementations under `UseCases/Impl/`.
- **`Frydek.People.Infrastructure`** — Concrete `PersonRepository` implementing `IPersonRepository`. This is where real persistence would land.
- **`Frydek.People.App`** — ASP.NET Core host: controllers, DI wiring, Program.cs, Swagger/OpenAPI configuration.

### Request flow

`Controller` → `IXxxUseCase` → `IPersonRepository` → returns `Person` entity → use case maps to `PersonDto` → controller returns HTTP response.

Controllers never reference the repository or entities directly — they only see use case interfaces and DTOs.

### Adding a new endpoint (pattern)

When adding an operation (e.g. `Update`), follow the existing structure:

1. **DTO** in `Application/Dtos/` if input differs from `PersonDto`.
2. **Interface** `IXxxPersonUseCase` in `Application/UseCases/`.
3. **Implementation** `XxxPersonUseCase` in `Application/UseCases/Impl/`. If the operation targets an existing entity, invoke `PersonRepository.GetById` first and throw `NotFoundException` when the entity is missing (see `DeletePersonUseCase` and `GetPersonUseCase` for the canonical shape).
4. **Repository method** in `Infrastructure/Repositories/PersonRepository.cs` (mock body: `await Task.CompletedTask;`).
5. **Controller action** in `App/Controllers/PersonController.cs`. Wrap use case calls in `try/catch (NotFoundException)` returning `NotFound(e.Message)`.
6. **Register** the use case in `App/Infrastructure/Extensions/DependencyInjectionExtensions.cs` under `RegisterUseCases` (`AddScoped<IXxx, Xxx>()`).

### Dependency injection

All wiring lives in `Frydek.People.App/Infrastructure/Extensions/DependencyInjectionExtensions.cs`. `Program.cs` calls the single `RegisterDependencies` extension — no service registration should be added directly to `Program.cs`. Repositories and use cases are registered as `Scoped`.

### Constructor style

Both use cases and controllers use C# 12 primary constructors that re-expose their dependencies as private properties (e.g. `private IPersonRepository PersonRepository { get; } = personRepository;`). Follow this convention when adding new classes.

### Error handling

Domain-level "not found" is signalled by throwing `NotFoundException` from Core. Controllers translate it to HTTP `404`. There is no global exception filter — each controller action handles its own translation.

## API surface

Base route: `api/person`

- `GET /api/person` — list all (mock: 5 hard-coded people)
- `GET /api/person/{id}` — fetch by id (mock: always returns a person with the requested id)
- `POST /api/person` — create; body `CreatePersonDto`
- `DELETE /api/person/{id}` — delete (validates existence via `GetById` first)

## Notes

- Swagger UI and the built-in OpenAPI document (`MapOpenApi`) are both enabled only in `Development`.
- Local git identity for this repo is configured via `git config --local` — do not modify the global git config.
