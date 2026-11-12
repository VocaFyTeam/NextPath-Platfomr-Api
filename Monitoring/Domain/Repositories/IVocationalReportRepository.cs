using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IVocationalReportRepository : IBaseRepository<VocationalReport>
{
    Task<IEnumerable<VocationalReport>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId, int? studentId,
        CancellationToken cancellationToken);
}
