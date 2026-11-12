using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IHistoricalProfileRepository : IBaseRepository<HistoricalProfile>
{
    Task<HistoricalProfile?> FindByIdAsync(int id, CancellationToken cancellationToken);

    Task<HistoricalProfile?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    Task<HistoricalProfile?> FindByStudentIdAsync(Guid studentId, CancellationToken cancellationToken);
    Task<HistoricalProfile?> FindByStudentIdAsNoTrackingAsync(Guid studentId, CancellationToken cancellationToken);
    Task<bool> ExistsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId, int? groupId,
        Guid? studentId, CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsync(Guid psychologistId,
        CancellationToken cancellationToken);
    Task<IEnumerable<HistoricalProfile>> FindByGroupIdAsNoTrackingAsync(int groupId,
        CancellationToken cancellationToken);
}
