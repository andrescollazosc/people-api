using Frydek.People.Application.Dtos;

namespace Frydek.People.Application.UseCases;

public interface IUpdatePersonUseCase
{
    Task<PersonDto> ExecuteAsync(Guid id, UpdatePersonDto dto);
}
