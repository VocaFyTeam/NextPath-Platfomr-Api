using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class GroupComparisonReportResourceFromEntityAssembler
{
    public static GroupComparisonReportResource ToResourceFromEntity(GroupComparisonReport entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new GroupComparisonReportResource(
            entity.Id,
            entity.PsychologistId,
            entity.SelectedGroupIds.Select(groupId => groupId.Value).ToList(),
            entity.AggregatedConclusions,
            entity.CreatedAt);
    }
}
