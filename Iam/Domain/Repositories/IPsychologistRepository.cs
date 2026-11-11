using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Domain.Repositories;

public interface IPsychologistRepository : IBaseRepository<Psychologist>
{
    Task<Psychologist?> FindByIdAsNoTrackingAsync(PsychologistId id, CancellationToken cancellationToken);
    Task<Psychologist?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(PsychologistId id, CancellationToken cancellationToken);
    Task<bool> ExistsByUniqueLinkingCodeAsync(string linkingCode, CancellationToken cancellationToken);
}
