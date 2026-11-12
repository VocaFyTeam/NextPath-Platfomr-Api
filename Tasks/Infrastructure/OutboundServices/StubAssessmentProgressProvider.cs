using NextPath.Platform.Tasks.Application.Internal.OutboundServices;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Infrastructure.OutboundServices;

/// <summary>
///     TEMPORARY adapter: reports "0 tests completed" until the Assessments context publishes its facade.
///     While it is in place, the tests part (50 %) of the progress stays at 0.
/// </summary>
public class StubAssessmentProgressProvider : IAssessmentProgressProvider
{
    public Task<AssessmentProgressSnapshot> GetAssessmentProgressAsync(StudentId studentId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new AssessmentProgressSnapshot(0, 0));
    }
}
