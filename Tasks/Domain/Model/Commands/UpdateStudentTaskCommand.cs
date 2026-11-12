using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.Commands;

public record UpdateStudentTaskCommand(
    StudentTaskId StudentTaskId,
    string Title,
    DateOnly? DueDate,
    int Order,
    ETaskStatus Status);
