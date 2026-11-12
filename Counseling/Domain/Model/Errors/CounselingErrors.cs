using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Errors;

public static class CounselingErrors
{
    public static readonly Error SessionNotFound =
        Error.NotFound("Counseling.SessionNotFound", "The specified session was not found.");

    public static readonly Error PsychologistRoleRequired =
        Error.Forbidden("Counseling.PsychologistRoleRequired", "Only psychologists can manage sessions.");

    public static readonly Error NotSessionHost =
        Error.Forbidden("Counseling.NotSessionHost", "Only the psychologist who scheduled the session can change it.");

    public static readonly Error InvalidSessionType =
        Error.Validation("Counseling.InvalidSessionType", "The session type must be 'INDIVIDUAL' or 'GROUP'.");

    public static readonly Error InvalidSessionStatus =
        Error.Validation("Counseling.InvalidSessionStatus",
            "The status must be 'SCHEDULED', 'COMPLETED' or 'CANCELLED'.");

    public static readonly Error InvalidSessionSchedule =
        Error.Validation("Counseling.InvalidSessionSchedule",
            "Send the date as dd/MM/yyyy (or yyyy-MM-dd) and the time as h:mm AM/PM (or HH:mm).");

    public static readonly Error InvalidSessionDuration =
        Error.Validation("Counseling.InvalidSessionDuration",
            $"The duration must be between {Session.MinDurationMinutes} and {Session.MaxDurationMinutes} minutes.");

    public static readonly Error SessionDateInPast =
        Error.Validation("Counseling.SessionDateInPast", "A session cannot be scheduled in the past.");

    public static readonly Error SchedulingConflictDetected =
        Error.Conflict("Counseling.SchedulingConflictDetected",
            "The psychologist or one of the students already has a session at that time.");

    public static readonly Error SessionNotScheduled =
        Error.Conflict("Counseling.SessionNotScheduled", "Only scheduled sessions can be changed.");

    public static readonly Error SessionTypeCannotChange =
        Error.Validation("Counseling.SessionTypeCannotChange",
            "The type of a session cannot change; cancel it and schedule a new one.");

    public static readonly Error NotAGroupSession =
        Error.Validation("Counseling.NotAGroupSession", "Participants can only be added to group sessions.");

    public static readonly Error InvalidParticipantsCount =
        Error.Validation("Counseling.InvalidParticipantsCount",
            $"A group session needs between {GroupSession.MinParticipants} and {GroupSession.MaxAllowedParticipants} students, within its limit.");

    public static readonly Error GroupSessionFull =
        Error.Conflict("Counseling.GroupSessionFull", "The group session already reached its participant limit.");

    public static readonly Error StudentAlreadyParticipant =
        Error.Conflict("Counseling.StudentAlreadyParticipant", "The student is already invited to this session.");

    public static readonly Error InsufficientProfilesForGrouping =
        Error.Validation("Counseling.InsufficientProfilesForGrouping",
            "At least two student profiles with a dominant area are needed to suggest groups.");

    public static Error StudentNotFound(Guid studentId) =>
        Error.NotFound("Counseling.StudentNotFound", $"The student '{studentId}' was not found.");

    public static Error InvalidSessionData(string detail) =>
        Error.Validation("Counseling.InvalidSessionData", detail);
}
