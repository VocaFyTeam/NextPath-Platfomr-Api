using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
public readonly record struct HistoricalProfileId(int Value) : IEntityId<int>
{
    public static implicit operator int(HistoricalProfileId id) => id.Value;
    public static implicit operator HistoricalProfileId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
