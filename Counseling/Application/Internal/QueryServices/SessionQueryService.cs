using VocaFy.NextPath.Platform.Counseling.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Counseling.Application.QueryServices;
using VocaFy.NextPath.Platform.Counseling.Domain.Model;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.QueryServices;

public class SessionQueryService(ISessionRepository sessionRepository, ExternalIamService externalIamService)
    : ISessionQueryService
{
    public async Task<Session?> Handle(GetSessionByIdQuery query, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
        if (session is null) return null;

        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(query.RequesterUserId,
            cancellationToken);
        if (psychologistId is not null) return session.IsHostedBy(psychologistId.Value) ? session : null;

        var studentId = await externalIamService.FetchStudentIdByUserId(query.RequesterUserId, cancellationToken);
        return studentId is not null && session.Involves(studentId.Value) ? session : null;
    }

    public async Task<IEnumerable<Session>> Handle(GetAllSessionsQuery query, CancellationToken cancellationToken)
    {
        StudentId? requestedStudentId = query.StudentId is { } sid ? new StudentId(sid) : null;
        PsychologistId? requestedPsychologistId = query.PsychologistId is { } pid ? new PsychologistId(pid) : null;

        // Psychologists: only the sessions they host.
        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(query.RequesterUserId,
            cancellationToken);
        if (psychologistId is not null)
        {
            if (requestedPsychologistId is not null && requestedPsychologistId != psychologistId)
                return Array.Empty<Session>();
            return await sessionRepository.FindAllAsync(psychologistId, requestedStudentId, query.GroupId,
                query.Status, cancellationToken);
        }

        // Students: only the sessions they are invited to.
        var studentId = await externalIamService.FetchStudentIdByUserId(query.RequesterUserId, cancellationToken);
        if (studentId is null || (requestedStudentId is not null && requestedStudentId != studentId))
            return Array.Empty<Session>();

        return await sessionRepository.FindAllAsync(requestedPsychologistId, studentId, query.GroupId, query.Status,
            cancellationToken);
    }

    public Result<IReadOnlyList<GroupingSuggestion>> Handle(SuggestOptimalGroupingQuery query)
    {
        if (query.MaxParticipants < GroupSession.MinParticipants ||
            query.MaxParticipants > GroupSession.MaxAllowedParticipants)
            return Result<IReadOnlyList<GroupingSuggestion>>.Failure(CounselingErrors.InvalidParticipantsCount,
                CounselingError.InvalidParticipantsCount);

        try
        {
            return Result<IReadOnlyList<GroupingSuggestion>>.Success(
                GroupSession.SuggestOptimalGrouping(query.Candidates, query.MaxParticipants));
        }
        catch (InvalidOperationException)
        {
            // "Insufficient Profiles for Grouping" of the event storming.
            return Result<IReadOnlyList<GroupingSuggestion>>.Failure(CounselingErrors.InsufficientProfilesForGrouping,
                CounselingError.InsufficientProfilesForGrouping);
        }
    }
}
