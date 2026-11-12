using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetGroupComparisonReportByIdQuery(GroupComparisonReportId GroupComparisonReportId)
{
    public GetGroupComparisonReportByIdQuery(int groupComparisonReportId)
        : this(new GroupComparisonReportId(groupComparisonReportId))
    {
    }
}
