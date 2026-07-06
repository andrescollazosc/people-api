using Frydek.People.Application.Dtos;

namespace Frydek.People.Application.UseCases;

public interface ICreatePersonUseCase
{
    Task<PersonDto> ExecuteAsync(CreatePersonDto dto);
}
