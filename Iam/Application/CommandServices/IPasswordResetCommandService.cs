using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Iam.Application.CommandServices;

public interface IPasswordResetCommandService
{
    Task<Result> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken);
}
