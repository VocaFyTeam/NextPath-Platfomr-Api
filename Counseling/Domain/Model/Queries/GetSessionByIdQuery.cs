using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;

/// <summary>Returns nothing when the requester is neither the host nor an invited student.</summary>
public record GetSessionByIdQuery(SessionId Id, Guid RequesterUserId);
