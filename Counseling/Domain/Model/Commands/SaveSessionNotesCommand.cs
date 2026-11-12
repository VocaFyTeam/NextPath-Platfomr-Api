namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

public record SaveSessionNotesCommand(Guid SessionId, Guid RequesterUserId, string? Notes);
