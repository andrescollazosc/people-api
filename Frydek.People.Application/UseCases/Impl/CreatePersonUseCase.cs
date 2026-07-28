using FluentValidation;
using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Core.Abstractions;

namespace Frydek.People.Application.UseCases.Impl;

public class CreatePersonUseCase(
    IValidator<CreatePersonDto> validator,
    IPersonRepository personRepository
) : ICreatePersonUseCase
{
    public async Task<PersonDto> ExecuteAsync(CreatePersonDto dto)
    {
        await validator.ValidateAndThrowAsync(dto);

        var person = dto.ToPerson();

        await personRepository.CreateAsync(person);

        return person.ToPersonDto();
    }
}
