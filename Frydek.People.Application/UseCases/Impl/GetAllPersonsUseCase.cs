using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Core.Abstractions;

namespace Frydek.People.Application.UseCases.Impl;

public class GetAllPersonsUseCase(
    IPersonRepository personRepository
) : IGetAllPersonsUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<IEnumerable<PersonDto>> ExecuteAsync()
    {
        var people = await PersonRepository.GetAllAsync();

        return people.Select(person => person.ToPersonDto());
    }
}
