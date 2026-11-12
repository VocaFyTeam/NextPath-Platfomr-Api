namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record StudentProfileResource(int Id, Guid StudentId, Guid PsychologistId, int? GroupId, IEnumerable<string> Flags, string Observation, string LastActivityDate);
