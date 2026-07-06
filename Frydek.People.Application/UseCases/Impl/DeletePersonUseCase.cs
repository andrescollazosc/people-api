using Frydek.People.Application.Repositories;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class DeletePersonUseCase(
    IPersonRepository personRepository
) : IDeletePersonUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task ExecuteAsync(Guid id)
    {
        var person = await PersonRepository.GetById(id);

        if (person == null)
        {
            throw new NotFoundException($"Person {id} was not found.");
        }

        await PersonRepository.Delete(person);
    }
}
