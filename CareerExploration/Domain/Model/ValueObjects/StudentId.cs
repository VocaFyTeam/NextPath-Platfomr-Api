using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>
///     Reference to a student that lives in the IAM context. This context only keeps the id
///     (never the IAM entity), exactly like <c>ProfileId</c> in Publishing (learning-center).
/// </summary>
public readonly record struct StudentId(int Value) : IEntityId<int>
{
    public static implicit operator int(StudentId id) => id.Value;
    public static implicit operator StudentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
