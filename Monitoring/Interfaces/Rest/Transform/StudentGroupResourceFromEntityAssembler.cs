using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class StudentGroupResourceFromEntityAssembler
{
    public static StudentGroupResource ToResourceFromEntity(StudentGroup entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new StudentGroupResource(entity.Id, entity.Name, entity.Description, entity.PsychologistId);
    }
}
