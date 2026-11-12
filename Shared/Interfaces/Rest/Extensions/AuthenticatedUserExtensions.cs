using Microsoft.AspNetCore.Mvc;

namespace VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;

public static class AuthenticatedUserExtensions
{
    public const string UserIdKey = "AuthenticatedUserId";

    public static Guid? GetAuthenticatedUserId(
        this ControllerBase controller)
    {
        return controller.HttpContext.Items[UserIdKey] is Guid userId
            ? userId
            : null;
    }
}
