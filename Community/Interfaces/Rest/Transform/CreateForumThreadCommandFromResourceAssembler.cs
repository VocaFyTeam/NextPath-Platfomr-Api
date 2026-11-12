using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CreateForumThreadCommandFromResourceAssembler
{
    public static CreateForumThreadCommand ToCommandFromResource(CreateThreadResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateForumThreadCommand(resource.CommunityId, resource.AuthorId, resource.AuthorName,
            resource.Content);
    }
}
