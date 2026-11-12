using VocaFy.NextPath.Platform.Counseling.Application.CommandServices;
using VocaFy.NextPath.Platform.Counseling.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Counseling.Domain.Model;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.CommandServices;

/// <summary>
///     Schedules, reschedules, cancels and completes counseling sessions. Business rules:
///     only psychologists schedule; only the host changes a session; students must exist in IAM;
///     no session in the past; and neither the psychologist nor any student can have two overlapping sessions
///     ("Scheduling Conflict Detected" of the event storming).
/// </summary>
public class SessionCommandService(
    ISessionRepository sessionRepository,
    ExternalIamService externalIamService,
    IUnitOfWork unitOfWork)
    : ISessionCommandService
{
    public async Task<Result<Session>> Handle(ScheduleIndividualSessionCommand command,
        CancellationToken cancellationToken)
    {
        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(command.RequesterUserId,
            cancellationToken);
        if (psychologistId is null)
            return Fail(CounselingErrors.PsychologistRoleRequired, CounselingError.PsychologistRoleRequired);

        StudentId[] studentIds = [new StudentId(command.StudentId)];
        var studentsCheck = await EnsureStudentsExistAsync(studentIds, cancellationToken);
        if (studentsCheck is not null) return studentsCheck;

        var scheduleCheck = await ValidateScheduleAsync(psychologistId.Value, studentIds, command.ScheduledDateTime,
            command.DurationMinutes, null, true, cancellationToken);
        if (scheduleCheck is not null) return scheduleCheck;

        return await CreateAsync(() => new IndividualSession(psychologistId.Value, studentIds[0], command.Title,
            command.ScheduledDateTime, command.DurationMinutes, command.Notes, command.MeetLink), cancellationToken);
    }

    public async Task<Result<Session>> Handle(ScheduleGroupSessionCommand command, CancellationToken cancellationToken)
    {
        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(command.RequesterUserId,
            cancellationToken);
        if (psychologistId is null)
            return Fail(CounselingErrors.PsychologistRoleRequired, CounselingError.PsychologistRoleRequired);

        var studentIds = ToStudentIds(command.StudentIds);
        var limit = command.MaxParticipants ?? GroupSession.MaxAllowedParticipants;
        if (!GroupSession.IsValidParticipantsCount(studentIds.Count, limit))
            return Fail(CounselingErrors.InvalidParticipantsCount, CounselingError.InvalidParticipantsCount);

        var studentsCheck = await EnsureStudentsExistAsync(studentIds, cancellationToken);
        if (studentsCheck is not null) return studentsCheck;

        var scheduleCheck = await ValidateScheduleAsync(psychologistId.Value, studentIds, command.ScheduledDateTime,
            command.DurationMinutes, null, true, cancellationToken);
        if (scheduleCheck is not null) return scheduleCheck;

        return await CreateAsync(() => new GroupSession(psychologistId.Value, studentIds, command.GroupId,
            command.GroupingCriteria, limit, command.Title, command.ScheduledDateTime, command.DurationMinutes,
            command.Notes, command.MeetLink), cancellationToken);
    }

    public async Task<Result<Session>> Handle(UpdateSessionCommand command, CancellationToken cancellationToken)
    {
        var hosted = await FindHostedSessionAsync(command.SessionId, command.RequesterUserId, cancellationToken);
        if (hosted.IsFailure) return hosted;
        var session = hosted.Value!;

        if (session.Type != command.Type)
            return Fail(CounselingErrors.SessionTypeCannotChange, CounselingError.SessionTypeCannotChange);

        // The web app cancels by sending the whole session with status CANCELLED.
        switch (command.Status)
        {
            case ESessionStatus.Cancelled:
                return await ChangeStatusAsync(session, s => s.SaveNotes(command.Notes).CancelSession(),
                    cancellationToken);
            case ESessionStatus.Completed:
                return await ChangeStatusAsync(session, s => s.SaveNotes(command.Notes).CompleteSession(),
                    cancellationToken);
            case ESessionStatus.ConflictDetected:
                return Fail(CounselingErrors.InvalidSessionStatus, CounselingError.InvalidSessionStatus);
        }

        if (!session.IsScheduled)
            return Fail(CounselingErrors.SessionNotScheduled, CounselingError.SessionNotScheduled);

        var studentIds = ToStudentIds(command.StudentIds);
        var limit = session is GroupSession current ? command.MaxParticipants ?? current.MaxParticipants : 1;
        var validCount = session is GroupSession
            ? GroupSession.IsValidParticipantsCount(studentIds.Count, limit)
            : studentIds.Count == 1;
        if (!validCount)
            return session is GroupSession
                ? Fail(CounselingErrors.InvalidParticipantsCount, CounselingError.InvalidParticipantsCount)
                : Fail(CounselingErrors.InvalidSessionData("An individual session needs exactly one student."),
                    CounselingError.InvalidSessionData);

        var studentsCheck = await EnsureStudentsExistAsync(studentIds, cancellationToken);
        if (studentsCheck is not null) return studentsCheck;

        var rescheduled = session.ScheduledDateTime != command.ScheduledDateTime;
        var scheduleCheck = await ValidateScheduleAsync(session.PsychologistId, studentIds, command.ScheduledDateTime,
            command.DurationMinutes, session.Id, rescheduled, cancellationToken);
        if (scheduleCheck is not null) return scheduleCheck;

        try
        {
            session.UpdateDetails(command.Title, command.ScheduledDateTime, command.DurationMinutes, command.Notes,
                command.MeetLink);
            switch (session)
            {
                case IndividualSession individual:
                    individual.ReassignStudent(studentIds[0]);
                    break;
                case GroupSession group when limit >= group.Participants.Count:
                    group.UpdateGroupInfo(command.GroupId, command.GroupingCriteria, limit);
                    group.ReplaceParticipants(studentIds);
                    break;
                case GroupSession group:
                    // Lower limit: drop students first so the new limit fits.
                    group.ReplaceParticipants(studentIds);
                    group.UpdateGroupInfo(command.GroupId, command.GroupingCriteria, limit);
                    break;
            }
        }
        catch (ArgumentException exception)
        {
            return Fail(CounselingErrors.InvalidSessionData(exception.Message), CounselingError.InvalidSessionData);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Session>.Success(session);
    }

    public async Task<Result<Session>> Handle(CancelSessionCommand command, CancellationToken cancellationToken)
    {
        var hosted = await FindHostedSessionAsync(command.SessionId, command.RequesterUserId, cancellationToken);
        if (hosted.IsFailure) return hosted;
        return await ChangeStatusAsync(hosted.Value!, s => s.CancelSession(), cancellationToken);
    }

    public async Task<Result<Session>> Handle(CompleteSessionCommand command, CancellationToken cancellationToken)
    {
        var hosted = await FindHostedSessionAsync(command.SessionId, command.RequesterUserId, cancellationToken);
        if (hosted.IsFailure) return hosted;
        return await ChangeStatusAsync(hosted.Value!, s => s.CompleteSession(), cancellationToken);
    }

    public async Task<Result<Session>> Handle(SaveSessionNotesCommand command, CancellationToken cancellationToken)
    {
        var hosted = await FindHostedSessionAsync(command.SessionId, command.RequesterUserId, cancellationToken);
        if (hosted.IsFailure) return hosted;
        var session = hosted.Value!;

        session.SaveNotes(command.Notes);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Session>.Success(session);
    }

    public async Task<Result<Session>> Handle(AddSessionParticipantCommand command,
        CancellationToken cancellationToken)
    {
        var hosted = await FindHostedSessionAsync(command.SessionId, command.RequesterUserId, cancellationToken);
        if (hosted.IsFailure) return hosted;

        if (hosted.Value is not GroupSession group)
            return Fail(CounselingErrors.NotAGroupSession, CounselingError.NotAGroupSession);
        if (!group.IsScheduled)
            return Fail(CounselingErrors.SessionNotScheduled, CounselingError.SessionNotScheduled);

        var studentId = new StudentId(command.StudentId);
        if (group.HasParticipant(studentId))
            return Fail(CounselingErrors.StudentAlreadyParticipant, CounselingError.StudentAlreadyParticipant);
        if (group.IsFull)
            return Fail(CounselingErrors.GroupSessionFull, CounselingError.GroupSessionFull);

        StudentId[] newcomer = [studentId];
        var studentsCheck = await EnsureStudentsExistAsync(newcomer, cancellationToken);
        if (studentsCheck is not null) return studentsCheck;

        var scheduleCheck = await ValidateScheduleAsync(group.PsychologistId, newcomer, group.ScheduledDateTime,
            group.DurationMinutes, group.Id, false, cancellationToken);
        if (scheduleCheck is not null) return scheduleCheck;

        group.AddParticipant(studentId);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Session>.Success(group);
    }

    private static Result<Session> Fail(Error error, CounselingError code) => Result<Session>.Failure(error, code);

    private static List<StudentId> ToStudentIds(IEnumerable<Guid> ids) =>
        ids.Where(id => id != Guid.Empty).Distinct().Select(id => new StudentId(id)).ToList();

    private async Task<Result<Session>> CreateAsync(Func<Session> factory, CancellationToken cancellationToken)
    {
        Session session;
        try
        {
            session = factory();
        }
        catch (ArgumentException exception)
        {
            return Fail(CounselingErrors.InvalidSessionData(exception.Message), CounselingError.InvalidSessionData);
        }

        await sessionRepository.AddAsync(session, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Session>.Success(session);
    }

    private async Task<Result<Session>> ChangeStatusAsync(Session session, Action<Session> transition,
        CancellationToken cancellationToken)
    {
        if (!session.IsScheduled)
            return Fail(CounselingErrors.SessionNotScheduled, CounselingError.SessionNotScheduled);

        transition(session);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Session>.Success(session);
    }

    /// <summary>Loads the session (tracked) and checks that the requester is the psychologist who hosts it.</summary>
    private async Task<Result<Session>> FindHostedSessionAsync(Guid sessionId, Guid requesterUserId,
        CancellationToken cancellationToken)
    {
        var session = await sessionRepository.FindByIdAsync(new SessionId(sessionId), cancellationToken);
        if (session is null) return Fail(CounselingErrors.SessionNotFound, CounselingError.SessionNotFound);

        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(requesterUserId,
            cancellationToken);
        if (psychologistId is null)
            return Fail(CounselingErrors.PsychologistRoleRequired, CounselingError.PsychologistRoleRequired);
        if (!session.IsHostedBy(psychologistId.Value))
            return Fail(CounselingErrors.NotSessionHost, CounselingError.NotSessionHost);

        return Result<Session>.Success(session);
    }

    /// <returns>A failure for the first student that does not exist in IAM, or null when all exist.</returns>
    private async Task<Result<Session>?> EnsureStudentsExistAsync(IEnumerable<StudentId> studentIds,
        CancellationToken cancellationToken)
    {
        foreach (var studentId in studentIds)
            if (!await externalIamService.ExistsStudent(studentId, cancellationToken))
                return Fail(CounselingErrors.StudentNotFound(studentId.Value), CounselingError.StudentNotFound);
        return null;
    }

    /// <returns>A failure when the duration, the date or the agenda is not valid, or null when it is.</returns>
    private async Task<Result<Session>?> ValidateScheduleAsync(PsychologistId psychologistId,
        IReadOnlyCollection<StudentId> studentIds, DateTime start, int durationMinutes, SessionId? excludingId,
        bool checkPast, CancellationToken cancellationToken)
    {
        if (!Session.IsValidDuration(durationMinutes))
            return Fail(CounselingErrors.InvalidSessionDuration, CounselingError.InvalidSessionDuration);

        if (checkPast && start < DateTime.Now)
            return Fail(CounselingErrors.SessionDateInPast, CounselingError.SessionDateInPast);

        // A session that started up to MaxDurationMinutes before can still be running at 'start'.
        var candidates = await sessionRepository.FindScheduledStartingBetweenAsync(
            start.AddMinutes(-Session.MaxDurationMinutes), start.AddMinutes(durationMinutes), excludingId,
            cancellationToken);

        var conflict = candidates.Any(other => other.OverlapsWith(start, durationMinutes) &&
                                               (other.IsHostedBy(psychologistId) || studentIds.Any(other.Involves)));
        return conflict
            ? Fail(CounselingErrors.SchedulingConflictDetected, CounselingError.SchedulingConflictDetected)
            : null;
    }
}
