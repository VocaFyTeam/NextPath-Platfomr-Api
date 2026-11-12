using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class JoinCommunityCommandFromResourceAssembler
{
    public static JoinCommunityCommand ToCommandFromResource(Guid communityId, JoinCommunityResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new JoinCommunityCommand(communityId, resource.StudentId);
    }
}
