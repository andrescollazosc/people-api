using Frydek.People.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Frydek.People.Infrastructure.Data;

public class EfUnitOfWork<T> : IUnitOfWork where T : DbContext
{
    private T DbContext { get; }

    public EfUnitOfWork(T dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task CommitAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        DbContext.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}