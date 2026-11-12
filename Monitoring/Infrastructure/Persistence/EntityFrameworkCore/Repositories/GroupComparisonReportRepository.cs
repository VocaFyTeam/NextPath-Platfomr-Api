using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GroupComparisonReportRepository(AppDbContext context) : BaseRepository<GroupComparisonReport>(context), IGroupComparisonReportRepository
{
    
    public async Task<GroupComparisonReport?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var reportId = new GroupComparisonReportId(id);
        return await Context.Set<GroupComparisonReport>()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }
    
    public async Task<GroupComparisonReport?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var reportId = new GroupComparisonReportId(id);
        return await Context.Set<GroupComparisonReport>()
            .AsNoTracking()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }
    
    public async Task<IEnumerable<GroupComparisonReport>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId,
        CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        return await Context.Set<GroupComparisonReport>()
            .AsNoTracking()
            .Where(report => report.PsychologistId == ownerId)
            .OrderBy(report => report.Id)
            .ToListAsync(cancellationToken);
    }
}
