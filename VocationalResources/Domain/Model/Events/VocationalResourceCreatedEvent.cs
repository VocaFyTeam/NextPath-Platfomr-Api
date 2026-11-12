using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Events;

/// <summary>
///     Raised when a psychologist publishes a resource (e.g. Notifications can tell linked students).
/// </summary>
public record VocationalResourceCreatedEvent(Guid VocationalResourceId, Guid PsychologistId, string Title) : IEvent;
