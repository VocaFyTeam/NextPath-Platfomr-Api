namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record VocationalReportResource(int Id, Guid StudentId, Guid PsychologistId, string ReportType, string ContentSummary, bool IsValidatedByPsychologist, DateTimeOffset? ValidatedAt, DateTimeOffset? CreatedAt);
