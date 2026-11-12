using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

/// <summary>
///     Strongly typed UUID identifier of a counseling session (sessions.session_id).
/// </summary>
public readonly record struct SessionId(Guid Value) : IEntityId<Guid>
{
    public static SessionId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(SessionId id) => id.Value;
    public static implicit operator SessionId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
