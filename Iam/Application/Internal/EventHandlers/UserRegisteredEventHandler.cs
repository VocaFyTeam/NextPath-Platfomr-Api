using VocaFy.NextPath.Platform.Iam.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.EventHandlers;

public class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    : IEventHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("New {Role} registered: {Email} ({UserId})",
            domainEvent.Role, domainEvent.Email, domainEvent.UserId);
        return Task.CompletedTask;
    }
}
