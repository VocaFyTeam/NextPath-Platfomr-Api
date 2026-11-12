using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IGroupComparisonReportRepository : IBaseRepository<GroupComparisonReport>
{
    Task<IEnumerable<GroupComparisonReport>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId,
        CancellationToken cancellationToken);
}
