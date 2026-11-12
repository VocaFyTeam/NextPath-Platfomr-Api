using VocaFy.NextPath.Platform.Community.Application.QueryServices;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.QueryServices;

public class CommentQueryService(ICommentRepository commentRepository) : ICommentQueryService
{
    public async Task<IEnumerable<Comment>> Handle(GetCommentsByForumThreadIdQuery query,
        CancellationToken cancellationToken)
    {
        return await commentRepository.FindByThreadIdAsNoTrackingAsync(query.ForumThreadId, cancellationToken);
    }
}
