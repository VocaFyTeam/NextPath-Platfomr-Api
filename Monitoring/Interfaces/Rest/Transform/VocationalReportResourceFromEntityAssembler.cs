using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;
public static class VocationalReportResourceFromEntityAssembler
{
    public static VocationalReportResource ToResourceFromEntity(VocationalReport entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new VocationalReportResource(
            entity.Id,
            entity.StudentId,
            entity.PsychologistId,
            EnumResourceMapper.ToResource(entity.ReportType),
            entity.ContentSummary,
            entity.IsValidatedByPsychologist,
            entity.ValidatedAt,
            entity.CreatedAt);
    }
}
