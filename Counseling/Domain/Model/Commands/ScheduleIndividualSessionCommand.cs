namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

/// <param name="RequesterUserId">Authenticated user; must be a psychologist (the host of the session).</param>
public record ScheduleIndividualSessionCommand(
    Guid RequesterUserId,
    Guid StudentId,
    string Title,
    DateTime ScheduledDateTime,
    int DurationMinutes,
    string? Notes,
    string? MeetLink);
