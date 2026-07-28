using Frydek.People.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frydek.People.Infrastructure.Mappings;

public class PeopleMapping : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Age)
            .IsRequired();
    }
}