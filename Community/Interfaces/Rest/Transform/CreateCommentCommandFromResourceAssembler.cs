using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CreateCommentCommandFromResourceAssembler
{
    public static CreateCommentCommand ToCommandFromResource(CreateCommentResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateCommentCommand(resource.ThreadId, resource.AuthorName, resource.Content);
    }
}
