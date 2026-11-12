using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CreateCommunityCommandFromResourceAssembler
{
    public static CreateCommunityCommand ToCommandFromResource(CreateCommunityResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateCommunityCommand(resource.Name, resource.Description);
    }
}
