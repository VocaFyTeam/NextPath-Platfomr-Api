using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user, Guid profileId)
    {
        ArgumentNullException.ThrowIfNull(user);
        return new UserResource(user.Id.Value, user.Email, user.Role.ToString().ToLowerInvariant(),
            user.IsSessionActive, user.CreatedAt, profileId);
    }
}
