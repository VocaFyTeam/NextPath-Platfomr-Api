namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;

/// <param name="Type">INDIVIDUAL or GROUP.</param>
/// <param name="StudentId">Only for individual sessions.</param>
/// <param name="Date">dd/MM/yyyy (the format the web app shows).</param>
/// <param name="Time">h:mm AM/PM (the format the web app shows).</param>
/// <param name="ScheduledDateTime">Same moment in ISO 8601 (yyyy-MM-ddTHH:mm:ss).</param>
/// <param name="Status">SCHEDULED, COMPLETED or CANCELLED.</param>
public record CounselingSessionResource(
    Guid Id,
    string Title,
    string Type,
    Guid PsychologistId,
    Guid? StudentId,
    string? GroupId,
    string? GroupingCriteria,
    int? MaxParticipants,
    IReadOnlyList<Guid> StudentIds,
    int AttendeesCount,
    string Date,
    string Time,
    string ScheduledDateTime,
    int DurationMinutes,
    string Status,
    string Notes,
    string MeetLink);
