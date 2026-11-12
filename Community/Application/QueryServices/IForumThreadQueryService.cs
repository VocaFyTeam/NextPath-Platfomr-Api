using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Community.Application.QueryServices;

public interface IForumThreadQueryService
{
    Task<IEnumerable<ForumThread>> Handle(GetAllForumThreadsQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<ForumThread>> Handle(GetForumThreadsByCommunityIdQuery query,
        CancellationToken cancellationToken);

    Task<ForumThread?> Handle(GetForumThreadByIdQuery query, CancellationToken cancellationToken);
}
