using Frydek.People.Core.Entities;

namespace Frydek.People.Application.Repositories;

public interface IPersonRepository
{
    Task Create(Person person);
    Task Update(Person person);
    Task Delete(Person person);
    Task<Person> GetById(Guid id);
    Task<IEnumerable<Person>> GetAll();
}