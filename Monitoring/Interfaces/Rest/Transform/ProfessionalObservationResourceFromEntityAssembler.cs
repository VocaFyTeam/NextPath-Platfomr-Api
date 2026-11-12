using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class ProfessionalObservationResourceFromEntityAssembler
{
    public static ProfessionalObservationResource ToResourceFromEntity(ProfessionalObservation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new ProfessionalObservationResource(
            entity.Id,
            entity.HistoricalProfileId,
            entity.StudentId,
            entity.PsychologistId,
            entity.ObservationText,
            entity.Agreements,
            entity.SessionDate,
            entity.DigitalSignature,
            entity.CreatedAt);
    }
}
