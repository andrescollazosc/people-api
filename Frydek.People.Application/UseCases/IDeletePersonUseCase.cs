namespace Frydek.People.Application.UseCases;

public interface IDeletePersonUseCase
{
    Task ExecuteAsync(Guid id);
}
