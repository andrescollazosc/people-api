using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class GetPersonUseCase(
    IPersonRepository personRepository
) : IGetPersonUseCase
{
    public async Task<PersonDto> ExecuteAsync(Guid id)
    {
        var person = await personRepository.GetByIdAsync(id);

        if (person is null)
        {
            throw new NotFoundException($"Person {id} was not found.");
        }

        return person.ToPersonDto();
    }
}