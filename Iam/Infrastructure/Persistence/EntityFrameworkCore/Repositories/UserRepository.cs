using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByIdAsNoTrackingAsync(UserId id, CancellationToken cancellationToken) =>
        await Context.Set<User>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = User.NormalizeEmail(email);
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task<User?> FindByEmailAsNoTrackingAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = User.NormalizeEmail(email);
        return await Context.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = User.NormalizeEmail(email);
        return await Context.Set<User>().AnyAsync(u => u.Email == normalized, cancellationToken);
    }
}
