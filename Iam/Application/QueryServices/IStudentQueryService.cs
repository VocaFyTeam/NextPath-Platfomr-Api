using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Iam.Application.QueryServices;

public interface IStudentQueryService
{
    Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken);
    Task<Student?> Handle(GetStudentByUserIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Student>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken);
}
