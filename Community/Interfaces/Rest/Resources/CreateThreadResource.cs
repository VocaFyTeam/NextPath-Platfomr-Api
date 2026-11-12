namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

public record CreateThreadResource(Guid CommunityId, Guid AuthorId, string AuthorName, string Content);
