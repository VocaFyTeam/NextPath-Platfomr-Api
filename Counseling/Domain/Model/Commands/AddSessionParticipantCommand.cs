namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

/// <summary>Adds a student to a scheduled group session the requester hosts.</summary>
public record AddSessionParticipantCommand(Guid SessionId, Guid RequesterUserId, Guid StudentId);
