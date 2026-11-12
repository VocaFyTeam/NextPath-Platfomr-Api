using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;

/// <summary>
///     "Group Session Scheduled" of the event storming (US28).
/// </summary>
public record GroupSessionScheduledEvent(
    Guid SessionId,
    Guid PsychologistId,
    IReadOnlyList<Guid> StudentIds,
    DateTime ScheduledDateTime) : IEvent;
