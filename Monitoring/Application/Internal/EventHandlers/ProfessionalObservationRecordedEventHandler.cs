using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class ProfessionalObservationRecordedEventHandler(ILogger<ProfessionalObservationRecordedEventHandler> logger) : IEventHandler<ProfessionalObservationRecordedEvent>
{
    public Task Handle(ProfessionalObservationRecordedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Professional observation recorded for student {StudentId} by psychologist {PsychologistId}", domainEvent.StudentId.Value, domainEvent.PsychologistId.Value);
        return Task.CompletedTask;
    }
}
