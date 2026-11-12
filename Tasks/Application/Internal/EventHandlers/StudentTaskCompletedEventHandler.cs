using Microsoft.Extensions.Logging;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Tasks.Application.Internal.EventHandlers;

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
