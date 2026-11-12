namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

public record CompleteSessionCommand(Guid SessionId, Guid RequesterUserId);
