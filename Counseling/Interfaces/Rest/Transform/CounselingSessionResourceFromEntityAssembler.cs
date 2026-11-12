using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Transform;

public static class CounselingSessionResourceFromEntityAssembler
{
    public static CounselingSessionResource ToResourceFromEntity(Session entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var group = entity as GroupSession;
        var studentIds = entity.StudentIds.Select(id => id.Value).ToList();

        return new CounselingSessionResource(
            entity.Id.Value,
            entity.Title,
            CounselingSessionFormat.ToApiType(entity.Type),
            entity.PsychologistId.Value,
            (entity as IndividualSession)?.StudentId.Value,
            group?.GroupId,
            group?.GroupingCriteria,
            group?.MaxParticipants,
            studentIds,
            studentIds.Count,
            CounselingSessionFormat.FormatDate(entity.ScheduledDateTime),
            CounselingSessionFormat.FormatTime(entity.ScheduledDateTime),
            CounselingSessionFormat.FormatIso(entity.ScheduledDateTime),
            entity.DurationMinutes,
            CounselingSessionFormat.ToApiStatus(entity.Status),
            entity.Notes,
            entity.MeetLink);
    }

    public static GroupingSuggestionResource ToResourceFromValueObject(GroupingSuggestion suggestion) =>
        new(suggestion.Criteria, suggestion.StudentIds.Select(id => id.Value).ToList());
}
