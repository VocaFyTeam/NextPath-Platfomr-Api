using VocaFy.NextPath.Platform.Tasks.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Transform;

public static class StudentTaskResourceFromEntityAssembler
{
    public static StudentTaskResource ToResourceFromEntity(StudentTask entity)
    {
        return new StudentTaskResource(
            entity.Id,
            entity.StudentId,
            entity.Title,
            TaskStatusMapper.ToResourceValue(entity.Status),
            entity.DueDate,
            entity.Order);
    }
}
