using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class GenerateGroupComparisonReportCommandFromResourceAssembler
{
    public static GenerateGroupComparisonReportCommand ToCommandFromResource(
        GenerateGroupComparisonReportResource resource, int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new GenerateGroupComparisonReportCommand(psychologistId, resource.GroupIds ?? Array.Empty<int>());
    }
}
