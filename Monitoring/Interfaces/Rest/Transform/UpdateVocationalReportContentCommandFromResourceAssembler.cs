using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class UpdateVocationalReportContentCommandFromResourceAssembler
{
    public static UpdateVocationalReportContentCommand ToCommandFromResource(
        UpdateVocationalReportContentResource resource, int vocationalReportId, int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new UpdateVocationalReportContentCommand(vocationalReportId, psychologistId, resource.ContentSummary);
    }
}
