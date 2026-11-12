namespace VocaFy.NextPath.Platform.Community.Domain.Model.Commands;

public record CreateCommentCommand(Guid ForumThreadId, string AuthorName, string Content);
