using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct PsychologistId(Guid Value) : IEntityId<Guid>
{
    public static implicit operator Guid(PsychologistId id) => id.Value;
    public static implicit operator PsychologistId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
