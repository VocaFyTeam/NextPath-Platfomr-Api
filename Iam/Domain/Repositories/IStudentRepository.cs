using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Domain.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<Student?> FindByIdAsNoTrackingAsync(StudentId id, CancellationToken cancellationToken);
    Task<Student?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<IEnumerable<Student>> FindByLinkedPsychologistCodeAsync(string linkingCode,
        CancellationToken cancellationToken);
}
