using NextPath.Platform.Shared.Domain.Model.Events;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Domain.Model.Events;

public class StudentTaskCompletedEvent(StudentId studentId, StudentTaskId studentTaskId) : IEvent
{
    public StudentId StudentId { get; } = studentId;
    public StudentTaskId StudentTaskId { get; } = studentTaskId;
}
