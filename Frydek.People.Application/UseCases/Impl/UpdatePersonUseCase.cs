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

        person.Update(dto.FirstName, dto.LastName, dto.Age, dto.Email);
        
        await personRepository.UpdateAsync(person);

        return person.ToPersonDto();
    }
}
