using NextPath.Platform.Monitoring.Application.QueryServices;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Queries;
using NextPath.Platform.Monitoring.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.QueryServices;

public class StudentGroupQueryService(IStudentGroupRepository groupRepository) : IStudentGroupQueryService
{

    public async Task<StudentGroup?> Handle(GetStudentGroupByIdQuery query, CancellationToken cancellationToken)
    {
        return await groupRepository.FindByIdAsNoTrackingAsync(query.StudentGroupId, cancellationToken);
    }


    public async Task<IEnumerable<StudentGroup>> Handle(GetAllStudentGroupsQuery query,
        CancellationToken cancellationToken)
    {
        return await groupRepository.FindByPsychologistIdAsNoTrackingAsync(query.PsychologistId, cancellationToken);
    }
}
