using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.Commands;

public record ChangeStudentTaskStatusCommand(StudentTaskId StudentTaskId, ETaskStatus Status);
