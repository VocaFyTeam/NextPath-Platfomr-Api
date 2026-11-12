namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

/// <summary>
///     SessionStatus of the class diagram (4.7.1.8). Stored as text in sessions.status.
/// </summary>
/// <remarks>
///     ConflictDetected is kept for parity with the diagram; a session that collides with another one is
///     rejected (409 SchedulingConflictDetected) instead of being stored with this status.
/// </remarks>
public enum ESessionStatus
{
    Scheduled,
    Completed,
    Cancelled,
    ConflictDetected
}
