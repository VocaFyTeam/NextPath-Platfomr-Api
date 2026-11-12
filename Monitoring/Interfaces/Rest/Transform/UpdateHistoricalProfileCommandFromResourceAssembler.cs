using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class UpdateHistoricalProfileCommandFromResourceAssembler
{
    public static UpdateHistoricalProfileCommand ToCommandFromResource(UpdateStudentProfileResource resource,
        int historicalProfileId, int psychologistId, IReadOnlyCollection<EStudentFlag>? flags)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new UpdateHistoricalProfileCommand(historicalProfileId, psychologistId, resource.Observation, flags,
            resource.GroupId);
    }
}
