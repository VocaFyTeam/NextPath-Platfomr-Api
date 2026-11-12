using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IGroupComparisonReportRepository : IBaseRepository<GroupComparisonReport>
{
    Task<GroupComparisonReport?> FindByIdAsync(int id, CancellationToken cancellationToken);

    Task<GroupComparisonReport?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupComparisonReport>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId,
        CancellationToken cancellationToken);
}
