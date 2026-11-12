using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<(User user, Guid profileId)>> Handle(SignUpCommand command, CancellationToken cancellationToken);

    Task<Result<(User user, Guid profileId, string token)>> Handle(SignInCommand command,
        CancellationToken cancellationToken);

    Task<Result> Handle(SignOutCommand command, CancellationToken cancellationToken);
}
