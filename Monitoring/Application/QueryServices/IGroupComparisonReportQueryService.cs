using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;

public interface IGroupComparisonReportQueryService
{
    Task<GroupComparisonReport?> Handle(GetGroupComparisonReportByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<GroupComparisonReport>> Handle(GetAllGroupComparisonReportsQuery query,
        CancellationToken cancellationToken);
}
