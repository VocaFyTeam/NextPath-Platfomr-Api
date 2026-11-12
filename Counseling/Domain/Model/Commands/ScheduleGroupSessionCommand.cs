namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

/// <param name="RequesterUserId">Authenticated user; must be a psychologist (the host of the session).</param>
/// <param name="MaxParticipants">Optional limit; when null the group admits up to 30 students.</param>
public record ScheduleGroupSessionCommand(
    Guid RequesterUserId,
    IReadOnlyList<Guid> StudentIds,
    string? GroupId,
    string? GroupingCriteria,
    int? MaxParticipants,
    string Title,
    DateTime ScheduledDateTime,
    int DurationMinutes,
    string? Notes,
    string? MeetLink);
