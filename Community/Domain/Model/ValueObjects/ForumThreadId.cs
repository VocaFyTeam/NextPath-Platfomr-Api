using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;

public readonly record struct ForumThreadId(Guid Value) : IEntityId<Guid>
{
    public static ForumThreadId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(ForumThreadId id) => id.Value;
    public static implicit operator ForumThreadId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
