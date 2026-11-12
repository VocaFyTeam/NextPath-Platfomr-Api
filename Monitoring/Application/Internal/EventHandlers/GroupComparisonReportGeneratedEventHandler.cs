using NextPath.Platform.Monitoring.Domain.Model.Events;
using NextPath.Platform.Shared.Application.Internal.EventHandlers;

namespace NextPath.Platform.Monitoring.Application.Internal.EventHandlers;

public class GroupComparisonReportGeneratedEventHandler(ILogger<GroupComparisonReportGeneratedEventHandler> logger) : IEventHandler<GroupComparisonReportGeneratedEvent>
{
    public Task Handle(GroupComparisonReportGeneratedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Comparison of {GroupCount} groups generated for psychologist {PsychologistId}", domainEvent.GroupCount, domainEvent.PsychologistId.Value);
        return Task.CompletedTask;
    }
}
