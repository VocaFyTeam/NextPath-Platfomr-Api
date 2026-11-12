using NextPath.Platform.Shared.Domain.Model.Entities;

namespace NextPath.Platform.Monitoring.Domain.Model.Aggregates;

public partial class HistoricalProfile : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
