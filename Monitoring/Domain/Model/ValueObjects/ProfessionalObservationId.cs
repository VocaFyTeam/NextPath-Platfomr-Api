using NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct ProfessionalObservationId(int Value) : IEntityId<int>
{
    public static implicit operator int(ProfessionalObservationId id) => id.Value;
    public static implicit operator ProfessionalObservationId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
