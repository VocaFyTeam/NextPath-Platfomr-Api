using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class GenerateVocationalReportCommandFromResourceAssembler
{
    public static GenerateVocationalReportCommand ToCommandFromResource(GenerateVocationalReportResource resource,
        EReportType reportType, int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new GenerateVocationalReportCommand(resource.StudentId, psychologistId, reportType);
    }
}
