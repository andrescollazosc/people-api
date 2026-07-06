using Frydek.People.Application.Dtos;
using Frydek.People.Application.Repositories;
using Frydek.People.Core.Entities;

namespace Frydek.People.Application.UseCases.Impl;

public class CreatePersonUseCase(
    IPersonRepository personRepository
) : ICreatePersonUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<PersonDto> ExecuteAsync(CreatePersonDto dto)
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Age = dto.Age
        };

        await PersonRepository.Create(person);

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
