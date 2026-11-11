using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Repositories;

public interface IVocationalResourceRepository : IBaseRepository<VocationalResource>
{
    Task<VocationalResource?> FindByIdAsNoTrackingAsync(VocationalResourceId id, CancellationToken cancellationToken);

    /// <summary>Newest first (updated_at desc); null filters are ignored.</summary>
    Task<IEnumerable<VocationalResource>> FindAllAsync(PsychologistId? psychologistId, string? category,
        CancellationToken cancellationToken);

    Task<bool> ExistsByTitleAndPsychologistIdAsync(string title, PsychologistId psychologistId,
        VocationalResourceId? excludingId, CancellationToken cancellationToken);
}
