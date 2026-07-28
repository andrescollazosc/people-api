using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class DeletePersonUseCase(
    IPersonRepository personRepository,
    IUnitOfWork unitOfWork
) : IDeletePersonUseCase
{
    public async Task ExecuteAsync(Guid id)
    {
        var person = await personRepository.GetByIdAsync(id);

        if (person == null)
        {
            throw new NotFoundException($"Person {id} was not found.");
        }

        await personRepository.DeleteAsync(person);
        
        await unitOfWork.CommitAsync();
    }
}
