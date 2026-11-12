namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

public record CommentResource(Guid Id, Guid ThreadId, string AuthorName, string Content, DateTimeOffset? CreatedAt);
