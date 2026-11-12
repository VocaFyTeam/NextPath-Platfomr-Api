using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct VocationalReportId(int Value) : IEntityId<int>
{
    public static implicit operator int(VocationalReportId id) => id.Value;
    public static implicit operator VocationalReportId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
