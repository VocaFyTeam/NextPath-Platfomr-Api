using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetGroupComparisonReportByIdQuery(GroupComparisonReportId GroupComparisonReportId)
{
    public GetGroupComparisonReportByIdQuery(int groupComparisonReportId)
        : this(new GroupComparisonReportId(groupComparisonReportId))
    {
    }
}
