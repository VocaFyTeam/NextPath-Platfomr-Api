using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class CreateHistoricalProfileCommandFromResourceAssembler
{
    public static CreateHistoricalProfileCommand ToCommandFromResource(CreateStudentProfileResource resource,
        Guid psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateHistoricalProfileCommand(resource.StudentId, psychologistId, resource.GroupId,
            resource.Observation ?? string.Empty);
    }
}
