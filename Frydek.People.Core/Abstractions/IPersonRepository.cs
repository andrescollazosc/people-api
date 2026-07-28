using Frydek.People.Core.Entities;

namespace Frydek.People.Core.Abstractions;

public interface IPersonRepository
{
    Task CreateAsync(Person person);
    Task DeleteAsync(Person person);
    Task<Person?> GetByIdAsync(Guid id);
    Task<IEnumerable<Person>> GetAllAsync();
}