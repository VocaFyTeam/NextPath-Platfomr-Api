using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.EventHandlers;

public class SessionCancelledEventHandler(ILogger<SessionCancelledEventHandler> logger)
    : IEventHandler<SessionCancelledEvent>
{
    public Task Handle(SessionCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Session {SessionId} of {ScheduledDateTime} cancelled by psychologist " +
                              "{PsychologistId}; {StudentsCount} students affected",
            domainEvent.SessionId, domainEvent.ScheduledDateTime, domainEvent.PsychologistId,
            domainEvent.StudentIds.Count);
        return Task.CompletedTask;
    }
}
