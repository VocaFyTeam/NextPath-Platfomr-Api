using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Application.Internal.QueryServices;

public class TestResultQueryService(ITestResultRepository testResultRepository) : ITestResultQueryService
{
    public async Task<IEnumerable<TestResult>> Handle(GetAllTestResultsQuery query,
        CancellationToken cancellationToken)
    {
        return await testResultRepository.ListLatestFirstAsNoTrackingAsync(cancellationToken);
    }

    public async Task<IEnumerable<TestResult>> Handle(GetTestResultsByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await testResultRepository.FindByStudentIdAsNoTrackingAsync(query.StudentId, cancellationToken);
    }

    public async Task<TestResult?> Handle(GetTestResultByIdQuery query, CancellationToken cancellationToken)
    {
        return await testResultRepository.FindByResultIdAsNoTrackingAsync(query.TestResultId, cancellationToken);
    }
}
