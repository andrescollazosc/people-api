namespace Frydek.People.Core.Abstractions;

public interface IUnitOfWork
{
    Task CommitAsync();
    Task RollbackAsync();
    void Detach<T>(T entity) where T : class;
}