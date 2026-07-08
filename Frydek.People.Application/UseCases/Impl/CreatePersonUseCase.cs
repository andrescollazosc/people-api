using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Application.Repositories;

namespace Frydek.People.Application.UseCases.Impl;

public class CreatePersonUseCase(
    IPersonRepository personRepository
) : ICreatePersonUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<PersonDto> ExecuteAsync(CreatePersonDto dto)
    {
        var person = dto.ToPerson();

        await PersonRepository.Create(person);

        return person.ToPersonDto();
    }
}
