namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

/// <summary>Same body the frontend sends in community.store.js (threadId, authorName, content).</summary>
public record CreateCommentResource(Guid ThreadId, string AuthorName, string Content);
