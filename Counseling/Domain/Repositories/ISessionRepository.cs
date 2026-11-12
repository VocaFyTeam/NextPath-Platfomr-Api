using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Repositories;

public interface ISessionRepository : IBaseRepository<Session>
{
    Task<Session?> FindByIdAsNoTrackingAsync(SessionId id, CancellationToken cancellationToken);

    /// <summary>findByPsychologistId() of the diagram, in chronological order.</summary>
    Task<IEnumerable<Session>> FindByPsychologistIdAsync(PsychologistId psychologistId,
        CancellationToken cancellationToken);

    /// <summary>Chronological order; null filters are ignored.</summary>
    Task<IEnumerable<Session>> FindAllAsync(PsychologistId? psychologistId, StudentId? studentId, string? groupId,
        ESessionStatus? status, CancellationToken cancellationToken);

    /// <summary>
    ///     Scheduled sessions (of any psychologist) that start in [from, to). Used to detect schedule conflicts.
    /// </summary>
    Task<IEnumerable<Session>> FindScheduledStartingBetweenAsync(DateTime from, DateTime to,
        SessionId? excludingId, CancellationToken cancellationToken);
}
