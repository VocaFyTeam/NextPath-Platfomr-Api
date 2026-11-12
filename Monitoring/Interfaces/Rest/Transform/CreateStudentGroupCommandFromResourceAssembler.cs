using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class CreateStudentGroupCommandFromResourceAssembler
{
    public static CreateStudentGroupCommand ToCommandFromResource(CreateStudentGroupResource resource,
        int psychologistId)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateStudentGroupCommand(resource.Name, resource.Description ?? string.Empty, psychologistId);
    }
}
