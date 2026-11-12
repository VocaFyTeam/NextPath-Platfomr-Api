using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

public readonly record struct TestResultId(Guid Value) : IEntityId<Guid>
{
    public static TestResultId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(TestResultId id) => id.Value;
    public static implicit operator TestResultId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
