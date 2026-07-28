using Frydek.People.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Frydek.People.App.Infrastructure.DesignTime;

public class PeopleDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PeopleDbContext>
{
    public PeopleDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<PeopleDbContextDesignTimeFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DB_POSTGRES_PEOPLE")
            ?? throw new InvalidOperationException(
                "Connection string 'DB_POSTGRES_PEOPLE' is not configured. " +
                "Set it via 'dotnet user-secrets set \"ConnectionStrings:DB_POSTGRES_PEOPLE\" \"...\"' " +
                "or the ConnectionStrings__DB_POSTGRES_PEOPLE environment variable.");

        var options = new DbContextOptionsBuilder<PeopleDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PeopleDbContext(options);
    }
}
