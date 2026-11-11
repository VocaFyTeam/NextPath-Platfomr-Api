namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(Guid Id, string Email, string Role, Guid ProfileId, string Token);
