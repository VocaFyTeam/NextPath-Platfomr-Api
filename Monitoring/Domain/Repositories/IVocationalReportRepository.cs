using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IVocationalReportRepository : IBaseRepository<VocationalReport>
{
    Task<VocationalReport?> FindByIdAsync(int id, CancellationToken cancellationToken);

    Task<VocationalReport?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VocationalReport>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId, Guid? studentId,
        CancellationToken cancellationToken);
}
