using VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;
namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Notifications.Logging.Services;

public class LoggingPasswordResetNotificationService : IPasswordResetNotificationService
{
    public Task SendResetTokenAsync(string email, string plainToken, DateTimeOffset expirationDate,
        CancellationToken cancellationToken)
    {
        return Task.FromException(new InvalidOperationException(
            "Password reset delivery is not configured. Configure a private notification provider before enabling this endpoint."));
    }
}
