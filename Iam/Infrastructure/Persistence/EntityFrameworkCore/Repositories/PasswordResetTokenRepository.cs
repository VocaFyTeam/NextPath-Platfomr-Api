using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PasswordResetTokenRepository(AppDbContext context)
    : BaseRepository<PasswordResetToken>(context), IPasswordResetTokenRepository
{
    public async Task<PasswordResetToken?> FindByTokenHashAsync(string tokenHash,
        CancellationToken cancellationToken) =>
        await Context.Set<PasswordResetToken>().FirstOrDefaultAsync(t => t.Token == tokenHash, cancellationToken);
}
