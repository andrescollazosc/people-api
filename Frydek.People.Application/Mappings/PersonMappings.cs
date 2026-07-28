using Frydek.People.Application.Dtos;
using Frydek.People.Core.Entities;

namespace Frydek.People.Application.Mappings;

public static class PersonMappings
{
    public static Person ToPerson(this CreatePersonDto personDto)
    {
        return new Person(
            personDto.FirstName,
            personDto.LastName,
            personDto.Email,
            personDto.Age
        );
    }

    public static PersonDto ToPersonDto(this Person person)
    {
        return new PersonDto(
            person.Id,
            person.FirstName,
            person.LastName,
            person.Email,
            person.Age
        );
    }
}
