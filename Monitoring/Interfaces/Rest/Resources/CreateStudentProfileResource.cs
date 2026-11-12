namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateStudentProfileResource(Guid StudentId, int? GroupId, string? Observation);
