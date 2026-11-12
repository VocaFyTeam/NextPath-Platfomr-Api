using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

public readonly record struct TestQuestionId(Guid Value) : IEntityId<Guid>
{
    public static TestQuestionId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(TestQuestionId id) => id.Value;
    public static implicit operator TestQuestionId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
