namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

public record SignUpResource(
    string Email,
    string Password,
    string Role,
    string? SchoolOrUniversity,
    string? CurrentCycle,
    string? LinkedPsychologistCode,
    string? ProfessionalLicense,
    string? Specialty);
