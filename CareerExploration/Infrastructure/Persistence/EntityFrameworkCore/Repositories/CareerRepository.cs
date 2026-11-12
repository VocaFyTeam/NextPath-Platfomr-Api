using Microsoft.EntityFrameworkCore;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace VocaFy.NextPath.Platform.CareerExploration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CareerRepository(AppDbContext context) : BaseRepository<Career>(context), ICareerRepository
{
    public async Task<Career?> FindByCareerIdAsync(CareerId careerId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Career>()
            .AsNoTracking()
            .FirstOrDefaultAsync(career => career.Id == careerId, cancellationToken);
    }

    public async Task<IEnumerable<Career>> FindByCareerIdsAsync(IReadOnlyCollection<CareerId> careerIds,
        CancellationToken cancellationToken = default)
    {
        var ids = careerIds.ToArray();
        return await Context.Set<Career>()
            .AsNoTracking()
            .Where(career => ids.Contains(career.Id))
            .OrderBy(career => career.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Career>> FindByFiltersAsync(string? area, string? modality, int? durationYears,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Career>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(area)) query = query.Where(career => career.Area == area);
        if (!string.IsNullOrWhiteSpace(modality)) query = query.Where(career => career.Modality == modality);
        if (durationYears.HasValue) query = query.Where(career => career.DurationYears == durationYears.Value);

        return await query.OrderBy(career => career.Id).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Career>().AnyAsync(career => career.Name == name, cancellationToken);
    }
}
