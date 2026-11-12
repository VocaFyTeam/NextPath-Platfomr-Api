using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;

public partial class TestResult : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
