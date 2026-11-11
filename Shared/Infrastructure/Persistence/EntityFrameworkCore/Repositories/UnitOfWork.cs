using VocaFy.NextPath.Platform.Shared.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;


public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}