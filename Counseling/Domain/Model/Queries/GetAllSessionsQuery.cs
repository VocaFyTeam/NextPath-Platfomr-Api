using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;

/// <summary>
///     Sessions in chronological order. A psychologist only sees the sessions they host and a student only the
///     ones they are invited to; the optional filters narrow that set.
/// </summary>
public record GetAllSessionsQuery(
    Guid RequesterUserId,
    Guid? PsychologistId = null,
    Guid? StudentId = null,
    string? GroupId = null,
    ESessionStatus? Status = null);
