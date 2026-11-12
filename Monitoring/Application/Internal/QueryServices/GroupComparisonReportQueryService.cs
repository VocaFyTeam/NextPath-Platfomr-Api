using NextPath.Platform.Monitoring.Application.QueryServices;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Queries;
using NextPath.Platform.Monitoring.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.QueryServices;

public class GroupComparisonReportQueryService(IGroupComparisonReportRepository reportRepository)
    : IGroupComparisonReportQueryService
{

    public async Task<GroupComparisonReport?> Handle(GetGroupComparisonReportByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await reportRepository.FindByIdAsNoTrackingAsync(query.GroupComparisonReportId, cancellationToken);
    }
    
    public async Task<IEnumerable<GroupComparisonReport>> Handle(GetAllGroupComparisonReportsQuery query,
        CancellationToken cancellationToken)
    {
        return await reportRepository.FindByPsychologistIdAsNoTrackingAsync(query.PsychologistId, cancellationToken);
    }
}
