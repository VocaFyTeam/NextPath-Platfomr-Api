using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, Guid profileId, string token)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrEmpty(token);
        return new AuthenticatedUserResource(user.Id.Value, user.Email, user.Role.ToString().ToLowerInvariant(),
            profileId, token);
    }
}
