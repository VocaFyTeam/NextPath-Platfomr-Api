using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;

/// <summary>
///     created_at / updated_at, filled by the Shared AuditableEntityInterceptor.
/// </summary>
public abstract partial class Session : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
