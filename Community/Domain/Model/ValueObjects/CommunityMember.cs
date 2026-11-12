namespace VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;

/// <summary>
///     Membership of a student in a community (value object owned by <see cref="Aggregates.InterestCommunity" />).
/// </summary>
public class CommunityMember
{
    public CommunityMember()
    {
    }

    public CommunityMember(Guid studentId)
    {
        if (studentId == Guid.Empty) throw new ArgumentException("Student id is required.", nameof(studentId));
        StudentId = studentId;
        JoinedAt = DateTime.UtcNow;
    }

    public Guid StudentId { get; private set; }
    public DateTime JoinedAt { get; private set; }
}
