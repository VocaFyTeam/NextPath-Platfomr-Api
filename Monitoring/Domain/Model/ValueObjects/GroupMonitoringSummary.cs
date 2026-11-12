namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

public record GroupMonitoringSummary(
    StudentGroupId GroupId,
    string GroupName,
    int StudentCount,
    int InactiveCount,
    int InconsistentCount,
    int LowParticipationCount,
    int AtRiskCount);
