using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Community.Application.CommandServices;

public interface IForumThreadCommandService
{
    Task<Result<ForumThread>> Handle(CreateForumThreadCommand command, CancellationToken cancellationToken);

    Task<Result<ForumThread>> Handle(LikeForumThreadCommand command, CancellationToken cancellationToken);

    Task<Result<ForumThread>> Handle(ReportForumThreadCommand command, CancellationToken cancellationToken);
}
