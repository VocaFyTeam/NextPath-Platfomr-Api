using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Events;

/// <summary>
///     Raised when a student completes a vocational test. Other contexts (Monitoring, Reports,
///     Notifications) can react to it.
/// </summary>
public class TestResultCompletedEvent(
    TestResultId testResultId,
    Guid studentId,
    VocationalTestId vocationalTestId,
    string hollandCode) : IEvent
{
    public TestResultId TestResultId { get; } = testResultId;
    public Guid StudentId { get; } = studentId;
    public VocationalTestId VocationalTestId { get; } = vocationalTestId;
    public string HollandCode { get; } = hollandCode;
}
