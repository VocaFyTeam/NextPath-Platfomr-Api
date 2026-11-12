namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

public record PsychologistResource(
    Guid Id,
    Guid UserId,
    string ProfessionalLicense,
    string Specialty,
    string UniqueLinkingCode);
