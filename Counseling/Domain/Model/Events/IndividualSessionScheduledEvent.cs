using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;

/// <summary>
///     "Individual Session Scheduled" of the event storming (e.g. Notifications can remind the student, US13).
/// </summary>
public record IndividualSessionScheduledEvent(
    Guid SessionId,
    Guid PsychologistId,
    Guid StudentId,
    DateTime ScheduledDateTime) : IEvent;
