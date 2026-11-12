using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.QueryServices;

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
