using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

public readonly record struct VocationalTestId(Guid Value) : IEntityId<Guid>
{
    public static VocationalTestId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(VocationalTestId id) => id.Value;
    public static implicit operator VocationalTestId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
