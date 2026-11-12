using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;

/// <summary>
///     Full update (PUT) of a session the requester hosts. The type cannot change.
///     When <paramref name="Status" /> is Cancelled or Completed only that transition (and the notes) is applied,
///     which is how the web app cancels a session today.
/// </summary>
public record UpdateSessionCommand(
    Guid SessionId,
    Guid RequesterUserId,
    ESessionType Type,
    IReadOnlyList<Guid> StudentIds,
    string? GroupId,
    string? GroupingCriteria,
    int? MaxParticipants,
    string Title,
    DateTime ScheduledDateTime,
    int DurationMinutes,
    string? Notes,
    string? MeetLink,
    ESessionStatus? Status);
