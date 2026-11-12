namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;


public record GroupComparisonReportResource(int Id, Guid PsychologistId, IEnumerable<int> SelectedGroupIds, string AggregatedConclusions, DateTimeOffset? CreatedAt);
