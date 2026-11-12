using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.Internal.OutboundServices;

/// <summary>
///     Port towards the Assessments context. CareerExploration does not know how the vocational
///     profile is calculated: it only asks "how compatible is this student with this career?".
///     Returns <c>null</c> when the student has no completed vocational test yet.
///     The real adapter will wrap Luciana's <c>IAssessmentsContextFacade</c>.
/// </summary>
public interface IAssessmentCompatibilityProvider
{
    Task<int?> GetCompatibilityAsync(StudentId studentId, Career career, CancellationToken cancellationToken);
}
