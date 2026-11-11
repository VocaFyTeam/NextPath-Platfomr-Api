using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
namespace VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;

public readonly record struct UserId(Guid Value) : IEntityId<Guid>
{
    public static UserId New() => new(Guid.CreateVersion7());
    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}