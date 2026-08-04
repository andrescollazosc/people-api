using Frydek.People.Application.Dtos;
using Frydek.People.Core.Entities;

namespace Frydek.People.Application.Mappings;

public static class PersonMappings
{
    extension(CreatePersonDto personDto)
    {
        public Person ToPerson()
        {
            return new Person(
                personDto.FirstName,
                personDto.LastName,
                personDto.Email,
                personDto.Age
            );
        }
    }

    extension(Person person)
    {
        public PersonDto ToPersonDto()
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
}