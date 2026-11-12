using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class StudentGroupCreatedEventHandler(ILogger<StudentGroupCreatedEventHandler> logger) : IEventHandler<StudentGroupCreatedEvent>
{
    public Task Handle(StudentGroupCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Student group created: {Name} (psychologist {PsychologistId})", domainEvent.Name, domainEvent.PsychologistId.Value);
        return Task.CompletedTask;
    }
}
