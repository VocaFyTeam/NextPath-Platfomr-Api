using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.EventHandlers;

public class IndividualSessionScheduledEventHandler(ILogger<IndividualSessionScheduledEventHandler> logger)
    : IEventHandler<IndividualSessionScheduledEvent>
{
    public Task Handle(IndividualSessionScheduledEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Individual session {SessionId} scheduled by psychologist {PsychologistId} " +
                              "with student {StudentId} on {ScheduledDateTime}",
            domainEvent.SessionId, domainEvent.PsychologistId, domainEvent.StudentId, domainEvent.ScheduledDateTime);
        return Task.CompletedTask;
    }
}
