using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Assessments.Application.Acl;

public class AssessmentsContextFacade(ITestResultQueryService testResultQueryService) : IAssessmentsContextFacade
{
    public async Task<string> FetchLatestHollandCodeByStudentId(Guid studentId, CancellationToken cancellationToken)
    {
        var results = await testResultQueryService.Handle(new GetTestResultsByStudentIdQuery(studentId),
            cancellationToken);
        var latest = results.Where(result => result.IsCompleted)
            .OrderByDescending(result => result.CompletedAt)
            .FirstOrDefault();
        return latest?.Scores.HollandCode ?? string.Empty;
    }

    public async Task<int> CountCompletedTestsByStudentId(Guid studentId, CancellationToken cancellationToken)
    {
        var results = await testResultQueryService.Handle(new GetTestResultsByStudentIdQuery(studentId),
            cancellationToken);
        return results.Where(result => result.IsCompleted)
            .Select(result => result.VocationalTestId)
            .Distinct()
            .Count();
    }
}
