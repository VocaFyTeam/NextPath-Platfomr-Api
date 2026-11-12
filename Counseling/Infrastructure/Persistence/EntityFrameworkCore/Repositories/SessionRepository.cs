using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Counseling.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     SessionRepositoryImpl of the class diagram.
/// </summary>
public class SessionRepository(AppDbContext context) : BaseRepository<Session>(context), ISessionRepository
{
    public async Task<Session?> FindByIdAsNoTrackingAsync(SessionId id, CancellationToken cancellationToken) =>
        await Context.Set<Session>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IEnumerable<Session>> FindByPsychologistIdAsync(PsychologistId psychologistId,
        CancellationToken cancellationToken) =>
        await Context.Set<Session>().AsNoTracking()
            .Where(s => s.PsychologistId == psychologistId)
            .OrderBy(s => s.ScheduledDateTime)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Session>> FindAllAsync(PsychologistId? psychologistId, StudentId? studentId,
        string? groupId, ESessionStatus? status, CancellationToken cancellationToken)
    {
        var query = Context.Set<Session>().AsNoTracking();

        if (psychologistId is not null)
        {
            var hostId = psychologistId.Value;
            query = query.Where(s => s.PsychologistId == hostId);
        }

        if (status is not null)
        {
            var requestedStatus = status.Value;
            query = query.Where(s => s.Status == requestedStatus);
        }

        if (!string.IsNullOrWhiteSpace(groupId))
        {
            // Only group sessions have a group.
            var normalizedGroupId = groupId.Trim();
            var groups = await query.OfType<GroupSession>()
                .Where(g => g.GroupId == normalizedGroupId)
                .ToListAsync(cancellationToken);
            return FilterByStudent(groups, studentId).OrderBy(s => s.ScheduledDateTime).ToList();
        }

        if (studentId is null)
            return await query.OrderBy(s => s.ScheduledDateTime).ToListAsync(cancellationToken);

        // Individual sessions keep the student in a column, group sessions in session_participants.
        var attendeeId = studentId.Value;
        var individual = await query.OfType<IndividualSession>()
            .Where(s => s.StudentId == attendeeId)
            .ToListAsync(cancellationToken);
        var group = await query.OfType<GroupSession>()
            .Where(g => g.Participants.Any(p => p.StudentId == attendeeId))
            .ToListAsync(cancellationToken);

        return individual.Cast<Session>().Concat(group).OrderBy(s => s.ScheduledDateTime).ToList();
    }

    public async Task<IEnumerable<Session>> FindScheduledStartingBetweenAsync(DateTime from, DateTime to,
        SessionId? excludingId, CancellationToken cancellationToken)
    {
        var query = Context.Set<Session>().AsNoTracking()
            .Where(s => s.Status == ESessionStatus.Scheduled && s.ScheduledDateTime >= from &&
                        s.ScheduledDateTime < to);

        if (excludingId is not null)
        {
            var currentId = excludingId.Value;
            query = query.Where(s => s.Id != currentId);
        }

        return await query.ToListAsync(cancellationToken);
    }

    private static IEnumerable<Session> FilterByStudent(IEnumerable<GroupSession> sessions, StudentId? studentId) =>
        studentId is null ? sessions : sessions.Where(g => g.HasParticipant(studentId.Value));
}
