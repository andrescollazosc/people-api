using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frydek.People.Infrastructure.Repositories;

public class PersonRepository(PeopleDbContext dbContext) : IPersonRepository
{
    public async Task CreateAsync(Person person)
    {
        await dbContext.People.AddAsync(person);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        dbContext.People.Remove(person);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<Person?> GetByIdAsync(Guid id)
    {
        var person = await dbContext.People.SingleOrDefaultAsync(p => p.Id == id);

        return person;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        var people = await dbContext.People.ToListAsync();
        
        return people;
    }
}
