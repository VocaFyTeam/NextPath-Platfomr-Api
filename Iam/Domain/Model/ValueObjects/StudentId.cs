using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;

public readonly record struct StudentId(Guid Value) : IEntityId<Guid>
{
    public static StudentId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(StudentId id) => id.Value;
    public static implicit operator StudentId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
