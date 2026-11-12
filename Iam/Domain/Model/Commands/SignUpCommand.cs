namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;

public record SignUpCommand(
    string Email,
    string Password,
    string Role,
    string? SchoolOrUniversity,
    string? CurrentCycle,
    string? LinkedPsychologistCode,
    string? ProfessionalLicense,
    string? Specialty);
