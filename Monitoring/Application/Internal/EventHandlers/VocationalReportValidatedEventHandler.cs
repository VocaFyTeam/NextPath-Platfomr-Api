using NextPath.Platform.Monitoring.Domain.Model.Events;
using NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class VocationalReportValidatedEventHandler(ILogger<VocationalReportValidatedEventHandler> logger) : IEventHandler<VocationalReportValidatedEvent>
{
    public Task Handle(VocationalReportValidatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Report of student {StudentId} validated by psychologist {PsychologistId}", domainEvent.StudentId.Value, domainEvent.PsychologistId.Value);
        return Task.CompletedTask;
    }
}
