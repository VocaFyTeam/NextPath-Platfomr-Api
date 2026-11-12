using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Repositories;

public interface ITestResultRepository : IBaseRepository<TestResult>
{
    Task<TestResult?> FindByResultIdAsync(Guid id, CancellationToken cancellationToken);

    Task<TestResult?> FindByResultIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<TestResult>> FindByStudentIdAsNoTrackingAsync(Guid studentId, CancellationToken cancellationToken);

    Task<IEnumerable<TestResult>> ListLatestFirstAsNoTrackingAsync(CancellationToken cancellationToken);
}
