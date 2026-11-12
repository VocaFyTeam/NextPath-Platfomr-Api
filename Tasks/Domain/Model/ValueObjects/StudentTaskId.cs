using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

public readonly record struct StudentTaskId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentTaskId id) => id.Value;
    public static implicit operator StudentTaskId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
