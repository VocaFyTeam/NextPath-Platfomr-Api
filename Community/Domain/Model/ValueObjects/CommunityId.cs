using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;

public readonly record struct CommunityId(Guid Value) : IEntityId<Guid>
{
    public static CommunityId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(CommunityId id) => id.Value;
    public static implicit operator CommunityId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
