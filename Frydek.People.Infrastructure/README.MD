# PeopleDb Context Migrations

Run the commands below from this project's directory
(`Frydek.People.Infrastructure`). Relative paths avoid issues with spaces
in the repository path.

## Scaffold a Migration

```shell
dotnet ef migrations add CreatePeopleDb \
  --context PeopleDbContext \
  --startup-project ../Frydek.People.App/Frydek.People.App.csproj \
  --project ./Frydek.People.Infrastructure.csproj
```

## Remove Last Unapplied Migration

```shell
dotnet ef migrations remove \
  --context PeopleDbContext \
  --startup-project ../Frydek.People.App/Frydek.People.App.csproj \
  --project ./Frydek.People.Infrastructure.csproj
```

## Update Database

```shell
dotnet ef database update \
  --context PeopleDbContext \
  --startup-project ../Frydek.People.App/Frydek.People.App.csproj \
  --project ./Frydek.People.Infrastructure.csproj
```
