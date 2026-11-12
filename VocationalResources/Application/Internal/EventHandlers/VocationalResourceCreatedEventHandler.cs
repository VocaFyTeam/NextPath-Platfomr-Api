using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.VocationalResources.Application.Internal.EventHandlers;

public class VocationalResourceCreatedEventHandler(ILogger<VocationalResourceCreatedEventHandler> logger)
    : IEventHandler<VocationalResourceCreatedEvent>
{
    public Task Handle(VocationalResourceCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Resource '{Title}' ({ResourceId}) published by psychologist {PsychologistId}",
            domainEvent.Title, domainEvent.VocationalResourceId, domainEvent.PsychologistId);
        return Task.CompletedTask;
    }
}
