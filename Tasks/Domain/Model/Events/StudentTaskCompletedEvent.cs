using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.Events;

public class StudentTaskCompletedEvent(StudentId studentId, StudentTaskId studentTaskId) : IEvent
{
    public StudentId StudentId { get; } = studentId;
    public StudentTaskId StudentTaskId { get; } = studentTaskId;
}
