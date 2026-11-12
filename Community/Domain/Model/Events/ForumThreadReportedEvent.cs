using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Events;

/// <summary>
///     Alert for the moderation team (US35). Notifications context can subscribe to it.
/// </summary>
public class ForumThreadReportedEvent(ForumThreadId forumThreadId, CommunityId communityId, string reason) : IEvent
{
    public ForumThreadId ForumThreadId { get; } = forumThreadId;
    public CommunityId CommunityId { get; } = communityId;
    public string Reason { get; } = reason;
}
