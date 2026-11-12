namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;

/// <summary>
///     Body for POST and PUT, with the same fields the web app already sends to /counseling-sessions.
///     The psychologist is taken from the token, never from the body (psychologistId, psychologistName, target
///     and attendeesCount are ignored if present).
/// </summary>
/// <param name="Type">INDIVIDUAL or GROUP.</param>
/// <param name="StudentId">Student of an individual session (or send it as the only item of StudentIds).</param>
/// <param name="StudentIds">Students of a group session.</param>
/// <param name="GroupId">Optional student group of the psychologist (e.g. "A").</param>
/// <param name="Date">dd/MM/yyyy (also accepts yyyy-MM-dd).</param>
/// <param name="Time">h:mm AM/PM (also accepts HH:mm).</param>
/// <param name="DurationMinutes">15 to 240; 60 when omitted.</param>
/// <param name="Status">Only used by PUT: SCHEDULED, COMPLETED or CANCELLED.</param>
public record SaveCounselingSessionResource(
    string Type,
    string Title,
    Guid? StudentId,
    IReadOnlyList<Guid>? StudentIds,
    string? GroupId,
    string? GroupingCriteria,
    int? MaxParticipants,
    string Date,
    string Time,
    int? DurationMinutes,
    string? Notes,
    string? MeetLink,
    string? Status);
