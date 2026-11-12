using NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct PsychologistId(int Value) : IEntityId<int>
{
    public static implicit operator int(PsychologistId id) => id.Value;
    public static implicit operator PsychologistId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
