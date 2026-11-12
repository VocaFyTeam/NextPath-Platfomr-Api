using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Domain.Model.Events;

namespace NextPath.Platform.Monitoring.Domain.Model.Events;

public record HistoricalProfileCreatedEvent(StudentId StudentId, PsychologistId PsychologistId) : IEvent;
