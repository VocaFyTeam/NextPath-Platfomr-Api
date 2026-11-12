using NextPath.Platform.Tasks.Application.QueryServices;
using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.Queries;
using NextPath.Platform.Tasks.Domain.Repositories;

namespace NextPath.Platform.Tasks.Application.Internal.QueryServices;

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
