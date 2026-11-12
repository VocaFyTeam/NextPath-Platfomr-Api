using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Events;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;

/// <summary>
///     Post published by a student inside a community (community_posts in the database design).
///     Named ForumThread to avoid a clash with System.Threading.Thread.
/// </summary>
public partial class ForumThread : AggregateRoot
{
    public ForumThread()
    {
        AuthorName = string.Empty;
        Content = string.Empty;
        ReportReason = string.Empty;
    }

    public ForumThread(CreateForumThreadCommand command) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.AuthorName);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Content);
        Id = ForumThreadId.New();
        CommunityId = new CommunityId(command.CommunityId);
        AuthorId = command.AuthorId;
        AuthorName = command.AuthorName;
        Content = command.Content.Trim();
    }

    public ForumThreadId Id { get; }
    public CommunityId CommunityId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; private set; }
    public string Content { get; private set; }
    public int CommentsCount { get; private set; }
    public int LikesCount { get; private set; }
    public bool IsReported { get; private set; }
    public string ReportReason { get; private set; }

    /// <summary>Called each time a comment is posted in this thread.</summary>
    public void RegisterComment() => CommentsCount++;

    /// <summary>US35 scenario 1: "Me gusta" increments the reactions counter.</summary>
    public void Like() => LikesCount++;

    /// <summary>US35 scenario 2: report inappropriate content and raise an alert for moderation.</summary>
    public void Report(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        IsReported = true;
        ReportReason = reason;
        AddDomainEvent(new ForumThreadReportedEvent(Id, CommunityId, reason));
    }
}
