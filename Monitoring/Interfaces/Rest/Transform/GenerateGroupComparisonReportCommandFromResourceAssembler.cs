using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class GenerateGroupComparisonReportCommandFromResourceAssembler
{
    public static GenerateGroupComparisonReportCommand ToCommandFromResource(
        GenerateGroupComparisonReportResource resource, Guid psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new GenerateGroupComparisonReportCommand(psychologistId, resource.GroupIds ?? Array.Empty<int>());
    }
}
