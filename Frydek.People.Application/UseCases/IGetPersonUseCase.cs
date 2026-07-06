using Frydek.People.Application.Dtos;

namespace Frydek.People.Application.UseCases;

public interface IGetPersonUseCase
{
    Task<PersonDto> ExecuteAsync(Guid id);
}