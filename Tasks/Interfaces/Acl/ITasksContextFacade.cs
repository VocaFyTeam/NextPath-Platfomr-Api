namespace NextPath.Platform.Tasks.Interfaces.Acl;

public record StudentTaskSummary(int CompletedTasks, int TotalTasks);

/// <summary>Public contract of the Tasks context (consumed by Monitoring, for example).</summary>
public interface ITasksContextFacade
{
    Task<StudentTaskSummary> FetchTaskSummaryByStudentId(int studentId, CancellationToken cancellationToken = default);
}
