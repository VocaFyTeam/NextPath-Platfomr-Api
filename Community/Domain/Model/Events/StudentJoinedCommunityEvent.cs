using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Events;

public class StudentJoinedCommunityEvent(CommunityId communityId, Guid studentId) : IEvent
{
    public CommunityId CommunityId { get; } = communityId;
    public Guid StudentId { get; } = studentId;
}
