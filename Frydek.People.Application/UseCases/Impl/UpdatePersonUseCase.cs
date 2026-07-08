using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Application.Repositories;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class UpdatePersonUseCase(
    IPersonRepository personRepository
) : IUpdatePersonUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<PersonDto> ExecuteAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await PersonRepository.GetById(id);

        if (person == null)
        {
            throw new NotFoundException($"Person {id} was not found.");
        }

        var updatedPerson = person with
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Age = dto.Age
        };

        await PersonRepository.Update(updatedPerson);

        return updatedPerson.ToPersonDto();
    }
}
