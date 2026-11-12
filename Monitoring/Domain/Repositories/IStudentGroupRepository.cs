using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

public interface IStudentGroupRepository : IBaseRepository<StudentGroup>
{
    Task<IEnumerable<StudentGroup>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId,
        CancellationToken cancellationToken);
 
    Task<IEnumerable<StudentGroup>> FindByIdsAsNoTrackingAsync(IEnumerable<StudentGroupId> ids,
        CancellationToken cancellationToken);
    
    Task<bool> ExistsByNameAndPsychologistIdAsync(string name, int psychologistId,
        CancellationToken cancellationToken);
}
