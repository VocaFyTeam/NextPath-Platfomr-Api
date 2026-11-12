using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IStudentGroupRepository : IBaseRepository<StudentGroup>
{
    Task<StudentGroup?> FindByIdAsync(int id, CancellationToken cancellationToken);

    Task<StudentGroup?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentGroup>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId,
        CancellationToken cancellationToken);
 
    Task<IEnumerable<StudentGroup>> FindByIdsAsNoTrackingAsync(IEnumerable<StudentGroupId> ids,
        CancellationToken cancellationToken);
    
    Task<bool> ExistsByNameAndPsychologistIdAsync(string name, Guid psychologistId,
        CancellationToken cancellationToken);
}
