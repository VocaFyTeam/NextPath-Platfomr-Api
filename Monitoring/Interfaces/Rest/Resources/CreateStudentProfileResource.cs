namespace NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateStudentProfileResource(int StudentId, int? GroupId, string? Observation);
