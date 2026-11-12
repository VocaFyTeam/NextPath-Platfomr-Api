using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Queries;

namespace NextPath.Platform.Monitoring.Application.QueryServices;

public interface IStudentGroupQueryService
{
    Task<StudentGroup?> Handle(GetStudentGroupByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<StudentGroup>> Handle(GetAllStudentGroupsQuery query, CancellationToken cancellationToken);
}
