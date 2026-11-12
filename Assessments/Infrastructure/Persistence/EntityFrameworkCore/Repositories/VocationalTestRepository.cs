using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Assessments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VocationalTestRepository(AppDbContext context)
    : BaseRepository<VocationalTest>(context), IVocationalTestRepository
{
    public async Task<VocationalTest?> FindWithQuestionsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var testId = new VocationalTestId(id);
        return await Context.Set<VocationalTest>()
            .Include(test => test.Questions)
            .FirstOrDefaultAsync(test => test.Id == testId, cancellationToken);
    }

    public async Task<VocationalTest?> FindWithQuestionsByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var testId = new VocationalTestId(id);
        return await Context.Set<VocationalTest>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(test => test.Questions)
            .FirstOrDefaultAsync(test => test.Id == testId, cancellationToken);
    }

    public async Task<VocationalTest?> FindByTestIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        var testId = new VocationalTestId(id);
        return await Context.Set<VocationalTest>()
            .AsNoTracking()
            .FirstOrDefaultAsync(test => test.Id == testId, cancellationToken);
    }

    public async Task<IEnumerable<VocationalTest>> ListOrderedAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<VocationalTest>()
            .AsNoTracking()
            .OrderBy(test => test.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await Context.Set<VocationalTest>().AnyAsync(test => test.Title == title, cancellationToken);
    }
}
