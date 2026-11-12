using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class AuthenticatedUserAssembler
{
    public static async Task<Guid?> GetAuthenticatedPsychologistIdAsync(HttpContext httpContext)
    {
        if (httpContext.Items[AuthenticatedUserExtensions.UserIdKey] is not Guid userId) return null;

        var iamContextFacade = httpContext.RequestServices.GetRequiredService<IIamContextFacade>();
        var psychologistId = await iamContextFacade.FetchPsychologistIdByUserId(userId, httpContext.RequestAborted);
        return psychologistId == Guid.Empty ? null : psychologistId;
    }
}
