using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;

/// <summary>
///     Raised when the psychologist cancels a session, so the invited students can be told.
/// </summary>
public record SessionCancelledEvent(
    Guid SessionId,
    Guid PsychologistId,
    IReadOnlyList<Guid> StudentIds,
    DateTime ScheduledDateTime) : IEvent;
