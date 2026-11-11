using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class PsychologistResourceFromEntityAssembler
{
    public static PsychologistResource ToResourceFromEntity(Psychologist psychologist)
    {
        ArgumentNullException.ThrowIfNull(psychologist);
        return new PsychologistResource(psychologist.Id.Value, psychologist.UserId.Value,
            psychologist.ProfessionalLicense, psychologist.Specialty, psychologist.UniqueLinkingCode);
    }
}
