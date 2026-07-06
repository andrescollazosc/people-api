using Frydek.People.Application.Dtos;
using Frydek.People.Application.Repositories;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class GetPersonUseCase(
    IPersonRepository personRepository
) : IGetPersonUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<PersonDto> ExecuteAsync(Guid id)
    {
        var person = await PersonRepository.GetById(id);

        if (person == null)
        {
            throw new NotFoundException($"Person {id} was not found.");
        }

        return new PersonDto
        (
            person.Id,
            person.FirstName,
            person.LastName,
            person.Email,
            person.Age
        );
    }
}