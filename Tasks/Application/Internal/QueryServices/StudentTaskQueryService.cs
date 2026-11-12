using VocaFy.NextPath.Platform.Tasks.Application.QueryServices;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Tasks.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Tasks.Application.Internal.QueryServices;

public class StudentTaskQueryService(IStudentTaskRepository studentTaskRepository) : IStudentTaskQueryService
{
    public async Task<IEnumerable<StudentTask>> Handle(GetStudentTasksByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await studentTaskRepository.FindByStudentIdAsync(query.StudentId, cancellationToken);
    }

    public async Task<StudentTask?> Handle(GetStudentTaskByIdQuery query, CancellationToken cancellationToken)
    {
        return await studentTaskRepository.FindByStudentTaskIdAsNoTrackingAsync(query.StudentTaskId,
            cancellationToken);
    }
}
