using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Domain.Repositories;

public interface IVocationalReportRepository : IBaseRepository<VocationalReport>
{
    Task<IEnumerable<VocationalReport>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId, int? studentId,
        CancellationToken cancellationToken);
}
