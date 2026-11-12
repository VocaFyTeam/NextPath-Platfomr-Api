using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PsychologistRepository(AppDbContext context)
    : BaseRepository<Psychologist>(context), IPsychologistRepository
{
    public async Task<Psychologist?> FindByIdAsNoTrackingAsync(PsychologistId id,
        CancellationToken cancellationToken) =>
        await Context.Set<Psychologist>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Psychologist?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        await Context.Set<Psychologist>().AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

    public async Task<bool> ExistsByIdAsync(PsychologistId id, CancellationToken cancellationToken) =>
        await Context.Set<Psychologist>().AnyAsync(p => p.Id == id, cancellationToken);

    public async Task<bool> ExistsByUniqueLinkingCodeAsync(string linkingCode, CancellationToken cancellationToken) =>
        await Context.Set<Psychologist>().AnyAsync(p => p.UniqueLinkingCode == linkingCode, cancellationToken);
}
