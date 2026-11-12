using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class VocationalReportGeneratedEventHandler(ILogger<VocationalReportGeneratedEventHandler> logger) : IEventHandler<VocationalReportGeneratedEvent>
{
    public Task Handle(VocationalReportGeneratedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("{ReportType} report generated for student {StudentId}", domainEvent.ReportType, domainEvent.StudentId.Value);
        return Task.CompletedTask;
    }
}
