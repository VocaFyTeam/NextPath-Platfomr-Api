using VocaFy.NextPath.Platform.Shared.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> 
    where T : class
{
    protected readonly AppDbContext Context = context;

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}