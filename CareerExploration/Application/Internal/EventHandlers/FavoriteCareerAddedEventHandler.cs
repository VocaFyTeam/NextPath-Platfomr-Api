using Microsoft.Extensions.Logging;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.Internal.EventHandlers;

public class FavoriteCareerAddedEventHandler(ILogger<FavoriteCareerAddedEventHandler> logger)
    : IEventHandler<FavoriteCareerAddedEvent>
{
    public Task Handle(FavoriteCareerAddedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Student {StudentId} added career {CareerId} to favorites",
            domainEvent.StudentId, domainEvent.CareerId);
        return Task.CompletedTask;
    }
}
