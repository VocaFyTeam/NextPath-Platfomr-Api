using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;

public interface IStudentGroupQueryService
{
    Task<StudentGroup?> Handle(GetStudentGroupByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<StudentGroup>> Handle(GetAllStudentGroupsQuery query, CancellationToken cancellationToken);
}
