using Frydek.People.Application.Dtos;

namespace Frydek.People.Application.UseCases;

public interface IGetAllPersonsUseCase
{
    Task<IEnumerable<PersonDto>> ExecuteAsync();
}
