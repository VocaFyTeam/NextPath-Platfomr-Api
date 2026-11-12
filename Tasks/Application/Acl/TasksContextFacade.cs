using NextPath.Platform.Tasks.Domain.Model.ValueObjects;
using NextPath.Platform.Tasks.Domain.Repositories;
using NextPath.Platform.Tasks.Interfaces.Acl;

namespace NextPath.Platform.Tasks.Application.Acl;

public class TasksContextFacade(IStudentTaskRepository studentTaskRepository) : ITasksContextFacade
{
    public async Task<StudentTaskSummary> FetchTaskSummaryByStudentId(int studentId,
        CancellationToken cancellationToken = default)
    {
        var tasks = (await studentTaskRepository.FindByStudentIdAsync(new StudentId(studentId), cancellationToken))
            .ToList();
        return new StudentTaskSummary(tasks.Count(task => task.IsCompleted), tasks.Count);
    }
}
