using Frydek.People.Application.Repositories;
using Frydek.People.Core.Entities;

namespace Frydek.People.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    public async Task Create(Person person)
    {
        await Task.CompletedTask;
    }

    public async Task Update(Person person)
    {
        await Task.CompletedTask;
    }

    public async Task Delete(Person person)
    {
        await Task.CompletedTask;
    }

    public async Task<Person> GetById(Guid id)
    {
        await Task.CompletedTask;

        return new Person
        {
            Id = id,
            FirstName = "Andres",
            LastName = "Collazos",
            Age = 30,
            Email = "andres.collazos@viirtue.com"
        };
    }

    public async Task<IEnumerable<Person>> GetAll()
    {
        await Task.CompletedTask;

        return new List<Person>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Andres",
                LastName = "Collazos",
                Age = 30,
                Email = "andres.collazos@viirtue.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ana",
                LastName = "Perez",
                Age = 25,
                Email = "ana.perez@viirtue.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Carlos",
                LastName = "Ramirez",
                Age = 40,
                Email = "carlos.ramirez@viirtue.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Lucia",
                LastName = "Gomez",
                Age = 28,
                Email = "lucia.gomez@viirtue.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Martinez",
                Age = 35,
                Email = "diego.martinez@viirtue.com"
            }
        };
    }
}