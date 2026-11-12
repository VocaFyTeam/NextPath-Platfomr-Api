using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;

/// <summary>
///     Strongly typed UUID identifier (VocationalResourceId).
/// </summary>
public readonly record struct VocationalResourceId(Guid Value) : IEntityId<Guid>
{
    public static VocationalResourceId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(VocationalResourceId id) => id.Value;
    public static implicit operator VocationalResourceId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
