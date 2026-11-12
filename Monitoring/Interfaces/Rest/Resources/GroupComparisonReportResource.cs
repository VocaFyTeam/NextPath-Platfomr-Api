namespace NextPath.Platform.Monitoring.Interfaces.Rest.Resources;


public record GroupComparisonReportResource(int Id, int PsychologistId, IEnumerable<int> SelectedGroupIds, string AggregatedConclusions, DateTimeOffset? CreatedAt);
