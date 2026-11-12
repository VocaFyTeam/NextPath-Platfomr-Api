using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;

public partial class GroupComparisonReport : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
