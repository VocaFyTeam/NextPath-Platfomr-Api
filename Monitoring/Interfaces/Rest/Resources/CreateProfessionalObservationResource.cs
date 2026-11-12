namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateProfessionalObservationResource(string ObservationText, string? Agreements, DateTimeOffset? SessionDate);
