# Frydek People API

Sample .NET 9 Web API used as a Clean Architecture playground and a
testbed for AI-assisted development workflows.

## Stack

.NET 9, ASP.NET Core, EF Core 9 + Npgsql (PostgreSQL), FluentValidation,
NUnit 4 + NSubstitute for tests.

## Structure

See [`CLAUDE.md`](CLAUDE.md) for the architecture overview, per-project
responsibilities, and conventions.

## Configure the connection string

The API expects a connection string named `DB_POSTGRES_PEOPLE`.

Via User Secrets (recommended for local dev):

```shell
dotnet user-secrets set "ConnectionStrings:DB_POSTGRES_PEOPLE" \
  "Host=localhost;Database=dbNameHere;Username=usernameHere;Password=passHere" \
  --project Frydek.People.App
```

Or as an environment variable:

```shell
export ConnectionStrings__DB_POSTGRES_PEOPLE="Host=localhost;Database=dbNameHere;Username=usernameHere;Password=passHere"
```

## Build, run, test

```shell
dotnet build
dotnet run --project Frydek.People.App
dotnet test
```

## EF Core migrations

See [`Frydek.People.Infrastructure/README.MD`](Frydek.People.Infrastructure/README.MD).

## AI agent instructions

Agent guidelines live under [`.ai/`](.ai/):

- [`.ai/architect.md`](.ai/architect.md)
- [`.ai/reviewer.md`](.ai/reviewer.md)
