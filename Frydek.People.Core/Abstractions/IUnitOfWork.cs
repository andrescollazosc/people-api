namespace Frydek.People.Core.Abstractions;

public interface IUnitOfWork
{
    void Commit();
    void Rollback();
    Task CommitAsync();
    Task RollbackAsync();
    Task Detach<T>(T entity) where T : class;
}