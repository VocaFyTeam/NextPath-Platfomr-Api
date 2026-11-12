using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class ThreadResourceFromEntityAssembler
{
    public static ThreadResource ToResourceFromEntity(ForumThread entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new ThreadResource(entity.Id.Value, entity.CommunityId.Value, entity.AuthorId, entity.AuthorName,
            entity.Content, entity.CommentsCount, entity.LikesCount, entity.IsReported, entity.CreatedAt);
    }
}
