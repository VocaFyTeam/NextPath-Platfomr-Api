namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record ProfessionalObservationResource(int Id, int StudentProfileId, Guid StudentId, Guid PsychologistId, string ObservationText, string Agreements, DateTimeOffset? SessionDate, string DigitalSignature, DateTimeOffset? CreatedAt);
