using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;

public readonly record struct PasswordResetTokenId(Guid Value) : IEntityId<Guid>
{
    public static PasswordResetTokenId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(PasswordResetTokenId id) => id.Value;
    public static implicit operator PasswordResetTokenId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
