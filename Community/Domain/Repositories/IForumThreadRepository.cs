using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Domain.Repositories;

public interface IForumThreadRepository : IBaseRepository<ForumThread>
{
    Task<ForumThread?> FindByThreadIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ForumThread?> FindByThreadIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ForumThread>> ListNewestFirstAsNoTrackingAsync(CancellationToken cancellationToken);

    Task<IEnumerable<ForumThread>> FindByCommunityIdAsNoTrackingAsync(Guid communityId,
        CancellationToken cancellationToken);
}
