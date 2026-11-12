namespace VocaFy.NextPath.Platform.Counseling.Domain.Model;

/// <summary>
///     Each name must exist as a key in Shared/Resources/Errors/ErrorMessages.resx.
/// </summary>
public enum CounselingError
{
    None,
    SessionNotFound,
    PsychologistRoleRequired,
    NotSessionHost,
    StudentNotFound,
    InvalidSessionData,
    InvalidSessionType,
    InvalidSessionStatus,
    InvalidSessionSchedule,
    InvalidSessionDuration,
    SessionDateInPast,
    SchedulingConflictDetected,
    SessionNotScheduled,
    SessionTypeCannotChange,
    NotAGroupSession,
    InvalidParticipantsCount,
    GroupSessionFull,
    StudentAlreadyParticipant,
    InsufficientProfilesForGrouping,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
