using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Assessments.Application.QueryServices;

public interface ITestResultQueryService
{
    Task<IEnumerable<TestResult>> Handle(GetAllTestResultsQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<TestResult>> Handle(GetTestResultsByStudentIdQuery query, CancellationToken cancellationToken);

    Task<TestResult?> Handle(GetTestResultByIdQuery query, CancellationToken cancellationToken);
}
