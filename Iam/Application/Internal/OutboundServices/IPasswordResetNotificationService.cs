namespace VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;

///     Delivers the password reset token to the user (e-mail in production).
public interface IPasswordResetNotificationService
{
    Task SendResetTokenAsync(string email, string plainToken, DateTimeOffset expirationDate,
        CancellationToken cancellationToken);
}
