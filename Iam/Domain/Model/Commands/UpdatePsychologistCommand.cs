namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;

public record UpdatePsychologistCommand(
    Guid PsychologistId,
    Guid RequesterUserId,
    string ProfessionalLicense,
    string Specialty);
