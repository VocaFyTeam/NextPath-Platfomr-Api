using System.Globalization;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class StudentProfileResourceFromEntityAssembler
{
    public static StudentProfileResource ToResourceFromEntity(HistoricalProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new StudentProfileResource(
            entity.Id,
            entity.StudentId,
            entity.AssignedPsychologistId,
            entity.GroupId is { } groupId ? groupId.Value : null,
            entity.Flags.Select(flag => EnumResourceMapper.ToResource(flag)).ToList(),
            entity.ObservationSummary,
            entity.LastActivityAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}
