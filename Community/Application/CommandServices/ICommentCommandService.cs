using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Community.Application.CommandServices;

public interface ICommentCommandService
{
    Task<Result<Comment>> Handle(CreateCommentCommand command, CancellationToken cancellationToken);
}
