using NextPath.Platform.Shared.Domain.Model.Entities;

namespace NextPath.Platform.Monitoring.Domain.Model.Aggregates;

public partial class StudentGroup : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
