using VocaFy.NextPath.Platform.Community.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.EventHandlers;

public class ForumThreadReportedEventHandler(ILogger<ForumThreadReportedEventHandler> logger)
    : IEventHandler<ForumThreadReportedEvent>
{
    public Task Handle(ForumThreadReportedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogWarning("Thread {ThreadId} of community {CommunityId} was reported. Reason: {Reason}",
            domainEvent.ForumThreadId.Value, domainEvent.CommunityId.Value, domainEvent.Reason);
        return Task.CompletedTask;
    }
}
