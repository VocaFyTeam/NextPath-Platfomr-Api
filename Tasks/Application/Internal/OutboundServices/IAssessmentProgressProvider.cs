using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Application.Internal.OutboundServices;

/// <param name="CompletedTests">Distinct vocational tests the student has completed.</param>
/// <param name="TotalTests">Vocational tests available in the platform catalog.</param>
public record AssessmentProgressSnapshot(int CompletedTests, int TotalTests);

/// <summary>
///     Port towards the Assessments context (owned by Luciana). Tasks does not know how tests
///     are stored: it only asks how many the student has completed.
/// </summary>
public interface IAssessmentProgressProvider
{
    Task<AssessmentProgressSnapshot> GetAssessmentProgressAsync(StudentId studentId,
        CancellationToken cancellationToken);
}
