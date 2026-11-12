using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new CommentResource(entity.Id.Value, entity.ForumThreadId.Value, entity.AuthorName, entity.Content,
            entity.CreatedAt);
    }
}
