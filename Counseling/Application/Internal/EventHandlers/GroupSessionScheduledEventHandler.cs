using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.EventHandlers;

public class GroupSessionScheduledEventHandler(ILogger<GroupSessionScheduledEventHandler> logger)
    : IEventHandler<GroupSessionScheduledEvent>
{
    public Task Handle(GroupSessionScheduledEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Group session {SessionId} scheduled by psychologist {PsychologistId} " +
                              "for {StudentsCount} students on {ScheduledDateTime}",
            domainEvent.SessionId, domainEvent.PsychologistId, domainEvent.StudentIds.Count,
            domainEvent.ScheduledDateTime);
        return Task.CompletedTask;
    }
}
