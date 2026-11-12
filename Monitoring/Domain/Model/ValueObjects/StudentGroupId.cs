using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct StudentGroupId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentGroupId id) => id.Value;
    public static implicit operator StudentGroupId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
