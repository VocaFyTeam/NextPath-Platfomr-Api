using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class CreateHistoricalProfileCommandFromResourceAssembler
{
    public static CreateHistoricalProfileCommand ToCommandFromResource(CreateStudentProfileResource resource,
        int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateHistoricalProfileCommand(resource.StudentId, psychologistId, resource.GroupId,
            resource.Observation ?? string.Empty);
    }
}
