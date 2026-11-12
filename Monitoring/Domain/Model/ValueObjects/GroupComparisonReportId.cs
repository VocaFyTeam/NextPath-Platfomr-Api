using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public readonly record struct GroupComparisonReportId(int Value) : IEntityId<int>
{
    public static implicit operator int(GroupComparisonReportId id) => id.Value;
    public static implicit operator GroupComparisonReportId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
