namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

public record StudentResource(
    Guid Id,
    Guid UserId,
    string SchoolOrUniversity,
    string CurrentCycle,
    string? LinkedPsychologistCode);
