using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;

/// <summary>
///     Comment written by a student in a <see cref="ForumThread" />.
/// </summary>
public partial class Comment : AggregateRoot
{
    public Comment()
    {
        AuthorName = string.Empty;
        Content = string.Empty;
    }

    public Comment(CreateCommentCommand command) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.AuthorName);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Content);
        Id = CommentId.New();
        ForumThreadId = new ForumThreadId(command.ForumThreadId);
        AuthorName = command.AuthorName;
        Content = command.Content.Trim();
    }

    public CommentId Id { get; }
    public ForumThreadId ForumThreadId { get; private set; }
    public string AuthorName { get; private set; }
    public string Content { get; private set; }
}
