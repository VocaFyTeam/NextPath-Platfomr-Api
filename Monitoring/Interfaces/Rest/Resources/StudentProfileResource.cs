namespace NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record StudentProfileResource(int Id, int StudentId, int PsychologistId, int? GroupId, IEnumerable<string> Flags, string Observation, string LastActivityDate);
