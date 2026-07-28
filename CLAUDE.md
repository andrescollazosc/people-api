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

Solution follows Clean Architecture with four projects. Dependency flow:
`App → Application → Core` and `App → Infrastructure → Core`.

- **Frydek.People.Core** — Domain. Entities (`Person`), abstractions
  (`IPersonRepository` under `Abstractions/`), domain exceptions
  (`NotFoundException`). No external dependencies.
- **Frydek.People.Application** — Use cases (one interface + impl per
  operation under `UseCases/` and `UseCases/Impl/`), DTOs (`Dtos/`),
  mappings (`Mappings/`), FluentValidation validators (`Validations/`,
  generic `PersonBaseDtoValidator<T>` shared by Create/Update).
- **Frydek.People.Infrastructure** — Repository implementations
  (`Repositories/`). References only `Core`.
- **Frydek.People.App** — ASP.NET Core Web API entry point.
  `Program.cs`, controllers (`Controllers/`), DI wiring
  (`Infrastructure/Extensions/DependencyInjectionExtensions.cs`), global
  exception handlers via `IExceptionHandler`
  (`Infrastructure/ExceptionHandlers/`).

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
├── Abstractions/              (IPersonRepository)
├── Entities/                  (Person as record)
└── Exceptions/                (NotFoundException)

Frydek.People.Infrastructure   (adapters)
└── Repositories/              (IPersonRepository impl)
```

### Stack and conventions

Stack: .NET 9, ASP.NET Core controllers, FluentValidation, native
`Microsoft.AspNetCore.OpenApi` (no Swashbuckle). Entities and DTOs use
`record` types; controllers and use cases use primary constructors.
