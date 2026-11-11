namespace VocaFy.NextPath.Platform.Shared.Domain.Repositories;

public interface IBaseRepository<TEntity>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : struct;

    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default);
}