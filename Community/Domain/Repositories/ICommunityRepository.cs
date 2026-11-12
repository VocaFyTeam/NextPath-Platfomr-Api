using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Domain.Repositories;

public interface ICommunityRepository : IBaseRepository<InterestCommunity>
{
    Task<InterestCommunity?> FindByCommunityIdAsync(Guid id, CancellationToken cancellationToken);

    Task<InterestCommunity?> FindByCommunityIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
}
