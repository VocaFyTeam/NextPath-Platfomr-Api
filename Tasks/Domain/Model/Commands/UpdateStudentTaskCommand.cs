using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Domain.Model.Commands;

public record UpdateStudentTaskCommand(
    StudentTaskId StudentTaskId,
    string Title,
    DateOnly? DueDate,
    int Order,
    ETaskStatus Status);
