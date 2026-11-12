using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class StudentResourceFromEntityAssembler
{
    public static StudentResource ToResourceFromEntity(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        return new StudentResource(student.Id.Value, student.UserId.Value, student.SchoolOrUniversity,
            student.CurrentCycle, student.LinkedPsychologistCode);
    }
}
