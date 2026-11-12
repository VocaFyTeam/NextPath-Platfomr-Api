using VocaFy.NextPath.Platform.Tasks.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Tasks.Application.QueryServices;

public interface IStudentTaskQueryService
{
    Task<IEnumerable<StudentTask>> Handle(GetStudentTasksByStudentIdQuery query, CancellationToken cancellationToken);

    Task<StudentTask?> Handle(GetStudentTaskByIdQuery query, CancellationToken cancellationToken);
}
