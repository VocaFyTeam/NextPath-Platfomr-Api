using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;

/// <summary>
///     psychologist_id of the IAM psychologists table (this context only keeps the reference).
/// </summary>
public readonly record struct PsychologistId(Guid Value) : IEntityId<Guid>
{
    public static PsychologistId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(PsychologistId id) => id.Value;
    public static implicit operator PsychologistId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
