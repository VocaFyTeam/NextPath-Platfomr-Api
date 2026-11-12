namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Acl;

/// <summary>
///     Public entry point of Assessments for other bounded contexts (Career Exploration, Monitoring...).
/// </summary>
public interface IAssessmentsContextFacade
{
    /// <summary>Returns the Holland code of the latest completed test, or an empty string.</summary>
    Task<string> FetchLatestHollandCodeByStudentId(Guid studentId, CancellationToken cancellationToken);

    /// <summary>Returns how many different tests the student has completed.</summary>
    Task<int> CountCompletedTestsByStudentId(Guid studentId, CancellationToken cancellationToken);
}
