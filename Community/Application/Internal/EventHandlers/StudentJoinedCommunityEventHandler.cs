using VocaFy.NextPath.Platform.Community.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.EventHandlers;

public class StudentJoinedCommunityEventHandler(ILogger<StudentJoinedCommunityEventHandler> logger)
    : IEventHandler<StudentJoinedCommunityEvent>
{
    public Task Handle(StudentJoinedCommunityEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Student {StudentId} joined community {CommunityId}",
            domainEvent.StudentId, domainEvent.CommunityId.Value);
        return Task.CompletedTask;
    }
}
