using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.VocationalResources.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VocationalResourceRepository(AppDbContext context)
    : BaseRepository<VocationalResource>(context), IVocationalResourceRepository
{
    public async Task<VocationalResource?> FindByIdAsNoTrackingAsync(VocationalResourceId id,
        CancellationToken cancellationToken) =>
        await Context.Set<VocationalResource>().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IEnumerable<VocationalResource>> FindAllAsync(PsychologistId? psychologistId, string? category,
        CancellationToken cancellationToken)
    {
        var query = Context.Set<VocationalResource>().AsNoTracking();

        if (psychologistId is not null)
        {
            var ownerId = psychologistId.Value;
            query = query.Where(r => r.PsychologistId == ownerId);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            var normalizedCategory = category.Trim();
            query = query.Where(r => r.Category == normalizedCategory);
        }

        return await query.OrderByDescending(r => r.UpdatedAt).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTitleAndPsychologistIdAsync(string title, PsychologistId psychologistId,
        VocationalResourceId? excludingId, CancellationToken cancellationToken)
    {
        var normalizedTitle = title.Trim();
        var query = Context.Set<VocationalResource>()
            .Where(r => r.Title == normalizedTitle && r.PsychologistId == psychologistId);

        if (excludingId is not null)
        {
            var currentId = excludingId.Value;
            query = query.Where(r => r.Id != currentId);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
