using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Domain.Repositories;

public interface IHistoricalProfileRepository : IBaseRepository<HistoricalProfile>
{

    Task<HistoricalProfile?> FindByStudentIdAsync(int studentId, CancellationToken cancellationToken);
    Task<HistoricalProfile?> FindByStudentIdAsNoTrackingAsync(int studentId, CancellationToken cancellationToken);
    Task<bool> ExistsByStudentIdAsync(int studentId, CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId, int? groupId,
        int? studentId, CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsync(int psychologistId,
        CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByGroupIdAsNoTrackingAsync(int groupId,
        CancellationToken cancellationToken);
}
