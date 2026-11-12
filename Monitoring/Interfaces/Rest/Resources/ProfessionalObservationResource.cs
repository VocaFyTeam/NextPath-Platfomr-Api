namespace NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record ProfessionalObservationResource(int Id, int StudentProfileId, int StudentId, int PsychologistId, string ObservationText, string Agreements, DateTimeOffset? SessionDate, string DigitalSignature, DateTimeOffset? CreatedAt);
