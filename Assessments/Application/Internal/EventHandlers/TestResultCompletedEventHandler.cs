using VocaFy.NextPath.Platform.Assessments.Domain.Model.Events;
using VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;
using Microsoft.Extensions.Logging;

namespace VocaFy.NextPath.Platform.Assessments.Application.Internal.EventHandlers;

public class TestResultCompletedEventHandler(ILogger<TestResultCompletedEventHandler> logger)
    : IEventHandler<TestResultCompletedEvent>
{
    public Task Handle(TestResultCompletedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Student {StudentId} completed vocational test {TestId} (result {ResultId}). Holland code: {Code}",
            domainEvent.StudentId, domainEvent.VocationalTestId.Value, domainEvent.TestResultId.Value, domainEvent.HollandCode);
        return Task.CompletedTask;
    }
}
