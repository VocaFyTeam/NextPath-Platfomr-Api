using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Events;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;

/// <summary>
///     Interest Community (ubiquitous language): space where students with similar vocational
///     affinities share experiences. Aggregate root that owns its members.
/// </summary>
public class InterestCommunity : AggregateRoot
{
    public InterestCommunity()
    {
        Name = string.Empty;
        Description = string.Empty;
        Members = new List<CommunityMember>();
    }

    public InterestCommunity(CreateCommunityCommand command) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);
        Id = CommunityId.New();
        Name = command.Name;
        Description = command.Description ?? string.Empty;
    }

    public CommunityId Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ICollection<CommunityMember> Members { get; }

    public bool HasMember(Guid studentId) => Members.Any(member => member.StudentId == studentId);

    /// <summary>
    ///     US33: a student joins the community. A student cannot join twice.
    /// </summary>
    public void Join(Guid studentId)
    {
        if (HasMember(studentId))
            throw new InvalidOperationException($"Student {studentId} is already a member of {Name}.");
        Members.Add(new CommunityMember(studentId));
        AddDomainEvent(new StudentJoinedCommunityEvent(Id, studentId));
    }
}
