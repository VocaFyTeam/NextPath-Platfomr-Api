using IamUser = VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates.User;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class AuthenticatedUserAssembler
{
    public static int? GetAuthenticatedUserId(HttpContext httpContext)
    {
        return httpContext.Items["User"] is IamUser user ? user.Id.Value : null;
    }
}
