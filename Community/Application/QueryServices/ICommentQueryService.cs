using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Community.Application.QueryServices;

public interface ICommentQueryService
{
    Task<IEnumerable<Comment>> Handle(GetCommentsByForumThreadIdQuery query, CancellationToken cancellationToken);
}
