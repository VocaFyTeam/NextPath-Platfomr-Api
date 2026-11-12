namespace NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record UpdateStudentProfileResource(string? Observation, IEnumerable<string>? Flags, int? GroupId);
