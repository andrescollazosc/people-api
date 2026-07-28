using Frydek.People.Core.Entities;
using Frydek.People.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Frydek.People.Infrastructure;

public class PeopleDbContext(DbContextOptions<PeopleDbContext> options) : DbContext(options)
{
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new PeopleMapping().Configure(modelBuilder.Entity<Person>());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

}