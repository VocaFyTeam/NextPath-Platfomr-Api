namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

public record ThreadResource(
    Guid Id,
    Guid CommunityId,
    Guid AuthorId,
    string AuthorName,
    string Content,
    int CommentsCount,
    int LikesCount,
    bool IsReported,
    DateTimeOffset? CreatedAt);
