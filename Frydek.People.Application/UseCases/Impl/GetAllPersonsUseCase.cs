using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Core.Abstractions;

namespace Frydek.People.Application.UseCases.Impl;

public class GetAllPersonsUseCase(
    IPersonRepository personRepository
) : IGetAllPersonsUseCase
{
    public async Task<IEnumerable<PersonDto>> ExecuteAsync()
    {
        var people = await personRepository.GetAllAsync();

        return people.Select(person => person.ToPersonDto());
    }
}
