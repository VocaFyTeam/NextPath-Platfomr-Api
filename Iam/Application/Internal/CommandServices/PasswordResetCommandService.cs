using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application;
using VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.CommandServices;

public class PasswordResetCommandService(
    IUserRepository userRepository,
    IPasswordResetTokenRepository passwordResetTokenRepository,
    IHashingService hashingService,
    IPasswordResetNotificationService notificationService,
    IUnitOfWork unitOfWork)
    : IPasswordResetCommandService
{
    public async Task<Result> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsNoTrackingAsync(command.Email, cancellationToken);

        if (user is null) return Result.Success();

        var plainToken = PasswordResetToken.GeneratePlainToken();
        var resetToken = new PasswordResetToken(user.Id, plainToken);

        await passwordResetTokenRepository.AddAsync(resetToken, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        await notificationService.SendResetTokenAsync(user.Email, plainToken, resetToken.ExpirationDate,
            cancellationToken);
        return Result.Success();
    }

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        if (!PasswordPolicy.IsValid(command.NewPassword))
            return Result.Failure(IamErrors.WeakPassword, IamError.WeakPassword);

        if (string.IsNullOrWhiteSpace(command.Token))
            return Result.Failure(IamErrors.InvalidOrExpiredResetToken, IamError.InvalidOrExpiredResetToken);

        var resetToken = await passwordResetTokenRepository.FindByTokenHashAsync(
            PasswordResetToken.HashToken(command.Token), cancellationToken);
        if (resetToken is null || !resetToken.IsValid(DateTimeOffset.UtcNow))
            return Result.Failure(IamErrors.InvalidOrExpiredResetToken, IamError.InvalidOrExpiredResetToken);

        var user = await userRepository.FindByIdAsync(resetToken.UserId, cancellationToken);
        if (user is null) return Result.Failure(IamErrors.UserNotFound, IamError.UserNotFound);

        user.ChangePassword(hashingService.HashPassword(command.NewPassword));
        resetToken.MarkAsUsed();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }
}
