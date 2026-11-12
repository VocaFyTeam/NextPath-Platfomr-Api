using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Assessments.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <remarks>
///     Scores, CareerMatches and Answers are owned types, so EF Core loads them automatically.
/// </remarks>
public class TestResultRepository(AppDbContext context) : BaseRepository<TestResult>(context), ITestResultRepository
{
    public async Task<TestResult?> FindByResultIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var resultId = new TestResultId(id);
        return await Context.Set<TestResult>()
            .FirstOrDefaultAsync(result => result.Id == resultId, cancellationToken);
    }

    public async Task<TestResult?> FindByResultIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        var resultId = new TestResultId(id);
        return await Context.Set<TestResult>()
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(result => result.Id == resultId, cancellationToken);
    }

    public async Task<IEnumerable<TestResult>> FindByStudentIdAsNoTrackingAsync(Guid studentId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<TestResult>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(result => result.StudentId == studentId)
            .OrderByDescending(result => result.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TestResult>> ListLatestFirstAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<TestResult>()
            .AsNoTracking()
            .AsSplitQuery()
            .OrderByDescending(result => result.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
