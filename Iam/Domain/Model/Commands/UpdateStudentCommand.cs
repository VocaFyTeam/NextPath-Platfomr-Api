namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;

public record UpdateStudentCommand(
    Guid StudentId,
    Guid RequesterUserId,
    string SchoolOrUniversity,
    string CurrentCycle,
    string? LinkedPsychologistCode);
