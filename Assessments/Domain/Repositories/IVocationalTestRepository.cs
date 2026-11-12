using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Repositories;

public interface IVocationalTestRepository : IBaseRepository<VocationalTest>
{
    /// <summary>Finds a test including its questions (tracked, for commands).</summary>
    Task<VocationalTest?> FindWithQuestionsByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Finds a test including its questions (read only, for queries).</summary>
    Task<VocationalTest?> FindWithQuestionsByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Finds a test without its questions (read only).</summary>
    Task<VocationalTest?> FindByTestIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Lists all tests ordered by the "Order" field.</summary>
    Task<IEnumerable<VocationalTest>> ListOrderedAsNoTrackingAsync(CancellationToken cancellationToken);

    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken);
}
