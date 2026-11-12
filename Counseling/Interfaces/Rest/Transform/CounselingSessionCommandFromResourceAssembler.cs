using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Transform;

/// <summary>
///     Builds commands from the body once the controller parsed type, date/time and status.
/// </summary>
public static class CounselingSessionCommandFromResourceAssembler
{
    /// <summary>StudentIds when sent, otherwise the single StudentId.</summary>
    public static IReadOnlyList<Guid> ToStudentIds(SaveCounselingSessionResource r)
    {
        if (r.StudentIds is { Count: > 0 }) return r.StudentIds.Where(id => id != Guid.Empty).Distinct().ToList();
        return r.StudentId is { } single && single != Guid.Empty ? new List<Guid> { single } : new List<Guid>();
    }

    public static ScheduleIndividualSessionCommand ToScheduleIndividualCommand(Guid requesterUserId,
        SaveCounselingSessionResource r, DateTime scheduledDateTime) =>
        new(requesterUserId, ToStudentIds(r)[0], r.Title, scheduledDateTime,
            r.DurationMinutes ?? Session.DefaultDurationMinutes, r.Notes, r.MeetLink);

    public static ScheduleGroupSessionCommand ToScheduleGroupCommand(Guid requesterUserId,
        SaveCounselingSessionResource r, DateTime scheduledDateTime) =>
        new(requesterUserId, ToStudentIds(r), r.GroupId, r.GroupingCriteria, r.MaxParticipants, r.Title,
            scheduledDateTime, r.DurationMinutes ?? Session.DefaultDurationMinutes, r.Notes, r.MeetLink);

    public static UpdateSessionCommand ToUpdateCommand(Guid sessionId, Guid requesterUserId,
        SaveCounselingSessionResource r, ESessionType type, DateTime scheduledDateTime, ESessionStatus? status) =>
        new(sessionId, requesterUserId, type, ToStudentIds(r), r.GroupId, r.GroupingCriteria, r.MaxParticipants,
            r.Title, scheduledDateTime, r.DurationMinutes ?? Session.DefaultDurationMinutes, r.Notes, r.MeetLink,
            status);

    public static SuggestOptimalGroupingQuery ToSuggestGroupingQuery(SuggestGroupingResource r) =>
        new((r.Students ?? Array.Empty<GroupingCandidateResource>())
                .Select(s => new GroupingCandidate(new StudentId(s.StudentId), s.DominantArea ?? string.Empty))
                .ToList(),
            r.MaxParticipants);
}
