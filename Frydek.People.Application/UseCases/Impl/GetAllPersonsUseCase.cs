using Frydek.People.Application.Dtos;
using Frydek.People.Application.Repositories;

namespace Frydek.People.Application.UseCases.Impl;

public class GetAllPersonsUseCase(
    IPersonRepository personRepository
) : IGetAllPersonsUseCase
{
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<IEnumerable<PersonDto>> ExecuteAsync()
    {
        var people = await PersonRepository.GetAll();

        return people.Select(person => new PersonDto
        (
            person.Id,
            person.FirstName,
            person.LastName,
            person.Email,
            person.Age
        ));
    }
}
