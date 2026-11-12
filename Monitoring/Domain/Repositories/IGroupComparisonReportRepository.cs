using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Domain.Repositories;

public interface IGroupComparisonReportRepository : IBaseRepository<GroupComparisonReport>
{
    Task<IEnumerable<GroupComparisonReport>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId,
        CancellationToken cancellationToken);
}
