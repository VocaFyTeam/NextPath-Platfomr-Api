using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class HistoricalProfileCreatedEventHandler(ILogger<HistoricalProfileCreatedEventHandler> logger) : IEventHandler<HistoricalProfileCreatedEvent>
{
    public Task Handle(HistoricalProfileCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Historical profile opened for student {StudentId} (psychologist {PsychologistId})", domainEvent.StudentId.Value, domainEvent.PsychologistId.Value);
        return Task.CompletedTask;
    }
}
