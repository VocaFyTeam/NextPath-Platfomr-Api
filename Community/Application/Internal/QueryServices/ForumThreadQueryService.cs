using VocaFy.NextPath.Platform.Community.Application.QueryServices;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.QueryServices;

public class ForumThreadQueryService(IForumThreadRepository forumThreadRepository) : IForumThreadQueryService
{
    public async Task<IEnumerable<ForumThread>> Handle(GetAllForumThreadsQuery query,
        CancellationToken cancellationToken)
    {
        return await forumThreadRepository.ListNewestFirstAsNoTrackingAsync(cancellationToken);
    }

    public async Task<IEnumerable<ForumThread>> Handle(GetForumThreadsByCommunityIdQuery query,
        CancellationToken cancellationToken)
    {
        return await forumThreadRepository.FindByCommunityIdAsNoTrackingAsync(query.CommunityId, cancellationToken);
    }

    public async Task<ForumThread?> Handle(GetForumThreadByIdQuery query, CancellationToken cancellationToken)
    {
        return await forumThreadRepository.FindByThreadIdAsNoTrackingAsync(query.ForumThreadId, cancellationToken);
    }
}
