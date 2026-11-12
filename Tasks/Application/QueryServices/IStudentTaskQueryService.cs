using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.Queries;

namespace NextPath.Platform.Tasks.Application.QueryServices;

public interface IStudentTaskQueryService
{
    Task<IEnumerable<StudentTask>> Handle(GetStudentTasksByStudentIdQuery query, CancellationToken cancellationToken);

    Task<StudentTask?> Handle(GetStudentTaskByIdQuery query, CancellationToken cancellationToken);
}
