using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

/// <summary>Reference to a student of the IAM context (id only, no navigation).</summary>
public readonly record struct StudentId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentId id) => id.Value;
    public static implicit operator StudentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
