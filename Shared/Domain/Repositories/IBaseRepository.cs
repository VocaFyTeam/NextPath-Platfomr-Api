namespace VocaFy.NextPath.Platform.Shared.Domain.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Remove(T entity);
}