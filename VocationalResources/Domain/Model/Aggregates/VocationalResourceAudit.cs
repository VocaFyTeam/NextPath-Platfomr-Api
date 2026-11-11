using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;

/// <summary>
///     created_at / updated_at, filled by the Shared AuditableEntityInterceptor.
/// </summary>
public partial class VocationalResource : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
