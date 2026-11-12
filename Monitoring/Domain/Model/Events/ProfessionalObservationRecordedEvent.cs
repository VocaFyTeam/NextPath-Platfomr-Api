using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;

public record ProfessionalObservationRecordedEvent(HistoricalProfileId HistoricalProfileId, StudentId StudentId, PsychologistId PsychologistId) : IEvent;
