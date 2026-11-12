using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class RecordProfessionalObservationCommandFromResourceAssembler
{
    public static RecordProfessionalObservationCommand ToCommandFromResource(
        CreateProfessionalObservationResource resource, int historicalProfileId, int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new RecordProfessionalObservationCommand(historicalProfileId, psychologistId,
            resource.ObservationText, resource.Agreements ?? string.Empty, resource.SessionDate);
    }
}
