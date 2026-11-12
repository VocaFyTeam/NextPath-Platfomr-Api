using NextPath.Platform.CareerExploration.Application.Internal.OutboundServices;
using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Infrastructure.OutboundServices;

/// <summary>
///     TEMPORARY adapter. Until the Assessments context publishes its facade, every student gets
///     the career's <c>BaseCompatibility</c> (the same fallback the frontend used).
///     To switch to the real thing, write <c>AssessmentCompatibilityProvider</c> (calling
///     <c>IAssessmentsContextFacade</c>) and change ONE line in <c>AddCareerExplorationContext</c>.
/// </summary>
public class StubAssessmentCompatibilityProvider : IAssessmentCompatibilityProvider
{
    public Task<int?> GetCompatibilityAsync(StudentId studentId, Career career, CancellationToken cancellationToken)
    {
        return Task.FromResult<int?>(career.BaseCompatibility);
    }
}
