using NextPath.Platform.Shared.Domain.Model.Entities;

namespace NextPath.Platform.Monitoring.Domain.Model.Entities;

public partial class ProfessionalObservation : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
