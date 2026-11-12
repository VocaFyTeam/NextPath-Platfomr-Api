using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.QueryServices;

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
