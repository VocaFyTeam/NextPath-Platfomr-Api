namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

public record CancelSessionCommand(Guid SessionId, Guid RequesterUserId);
