using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;

public readonly record struct CommentId(Guid Value) : IEntityId<Guid>
{
    public static CommentId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(CommentId id) => id.Value;
    public static implicit operator CommentId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
