# Claude Code Instructions

This repository contains AI agent instructions under the `.ai` directory.

When reviewing this project, always read and follow:

- `.ai/reviewer.md`
- `.ai/architect.md`

Default behavior:

- Do not modify files unless explicitly requested.
- Do not create commits.
- Do not push changes.
- Focus on .NET, Clean Architecture, maintainability, and code quality.

## Architecture

Solution follows Clean Architecture with four production projects plus a
unit-test project. Dependency flow: `App → Application → Core` and
`App → Infrastructure → Core`.

- **Frydek.People.Core** — Domain. Entity (`Person` class with
  encapsulated `Update()`), abstractions (`IPersonRepository`,
  `IUnitOfWork` under `Abstractions/`), domain exceptions
  (`NotFoundException`). No external dependencies.
- **Frydek.People.Application** — Use cases (one interface + impl per
  operation under `UseCases/` and `UseCases/Impl/`), DTOs (`Dtos/`),
  mappings (`Mappings/`), FluentValidation validators (`Validations/`,
  generic `PersonBaseDtoValidator<T>` shared by Create/Update).
- **Frydek.People.Infrastructure** — EF Core adapters. `PeopleDbContext`,
  repository implementations (`Repositories/`), unit of work
  (`Data/EfUnitOfWork`), entity configurations (`Mappings/`), EF Core
  migrations (`Migrations/`). References only `Core`.
- **Frydek.People.App** — ASP.NET Core Web API entry point.
  `Program.cs`, controllers (`Controllers/`), DI wiring
  (`Infrastructure/Extensions/DependencyInjectionExtensions.cs`), global
  exception handlers via `IExceptionHandler`
  (`Infrastructure/ExceptionHandlers/`).
- **Frydek.People.Application.Tests** — Unit tests for use cases under
  `UseCases/Unit/`, one file per use case. NUnit + NSubstitute.

### Project layout

```
Frydek.People.App              (ASP.NET Core Web API)
├── Controllers/
├── Infrastructure/
│   ├── ExceptionHandlers/
│   └── Extensions/            (DI registration)
└── Program.cs

Frydek.People.Application      (use cases, DTOs, validation)
├── Dtos/                      (PersonBaseDto + Create/Update/Response)
├── Mappings/                  (extension methods entity <-> DTO)
├── UseCases/                  (interfaces)
│   └── Impl/                  (implementations)
└── Validations/               (FluentValidation, generic base)

Frydek.People.Core             (domain, no dependencies)
├── Abstractions/              (IPersonRepository, IUnitOfWork)
├── Entities/                  (Person class with Update())
└── Exceptions/                (NotFoundException)

Frydek.People.Infrastructure   (EF Core adapters)
├── Data/                      (EfUnitOfWork)
├── Mappings/                  (EF entity configurations)
├── Migrations/                (EF Core migrations)
├── Repositories/              (IPersonRepository impl)
└── PeopleDbContext.cs

Frydek.People.Application.Tests (unit tests)
└── UseCases/
    └── Unit/                  (NUnit + NSubstitute, one file per use case)
```

### Stack and conventions

Stack: .NET 10, ASP.NET Core controllers, FluentValidation, native
`Microsoft.AspNetCore.OpenApi` (no Swashbuckle), EF Core 10 with Npgsql
(PostgreSQL) for persistence, NUnit 4 + NSubstitute for tests. DTOs use
`record` types; `Person` is a class with an encapsulated `Update()`
method. Controllers and use cases use primary constructors.
