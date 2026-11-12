namespace VocaFy.NextPath.Platform.Community.Domain.Model.Commands;

public record CreateForumThreadCommand(Guid CommunityId, Guid AuthorId, string AuthorName, string Content);
