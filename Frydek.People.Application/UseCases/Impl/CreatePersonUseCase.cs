using FluentValidation;
using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Application.Repositories;

namespace Frydek.People.Application.UseCases.Impl;

public class CreatePersonUseCase(
    IValidator<CreatePersonDto> validator,
    IPersonRepository personRepository
) : ICreatePersonUseCase
{
    private IValidator<CreatePersonDto> Validator { get; } = validator;
    private IPersonRepository PersonRepository { get; } = personRepository;

    public async Task<PersonDto> ExecuteAsync(CreatePersonDto dto)
    {
        await Validator.ValidateAndThrowAsync(dto);

        var person = dto.ToPerson();

        await PersonRepository.Create(person);

        return person.ToPersonDto();
    }
}
