using Microsoft.Extensions.Logging;
using NextPath.Platform.Shared.Application.Internal.EventHandlers;
using NextPath.Platform.Tasks.Domain.Model.Events;

namespace NextPath.Platform.Tasks.Application.Internal.EventHandlers;

public class StudentTaskCompletedEventHandler(ILogger<StudentTaskCompletedEventHandler> logger)
    : IEventHandler<StudentTaskCompletedEvent>
{
    public Task Handle(StudentTaskCompletedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Student {StudentId} completed task {StudentTaskId}",
            domainEvent.StudentId, domainEvent.StudentTaskId);
        return Task.CompletedTask;
    }
}
