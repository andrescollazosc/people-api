using FluentValidation;
using Frydek.People.Application.Dtos;
using Frydek.People.Application.Mappings;
using Frydek.People.Core.Abstractions;
using Frydek.People.Core.Exceptions;

namespace Frydek.People.Application.UseCases.Impl;

public class UpdatePersonUseCase(
    IValidator<UpdatePersonDto> validator,
    IPersonRepository personRepository
) : IUpdatePersonUseCase
{
    public async Task<PersonDto> ExecuteAsync(Guid id, UpdatePersonDto dto)
    {
        await validator.ValidateAndThrowAsync(dto);

        var person = await personRepository.GetByIdAsync(id);

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

        await personRepository.UpdateAsync(updatedPerson);

        return updatedPerson.ToPersonDto();
    }
}
