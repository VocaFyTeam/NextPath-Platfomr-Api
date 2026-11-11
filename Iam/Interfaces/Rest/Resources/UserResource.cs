namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(Guid Id, string Email, string Role, bool IsSessionActive, DateTimeOffset CreatedAt,
    Guid ProfileId);
