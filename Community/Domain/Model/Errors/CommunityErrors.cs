using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Community.Domain.Model.Errors;

public static class CommunityErrors
{
    public static readonly Error CommunityNotFound =
        Error.NotFound("Community.CommunityNotFound", "The specified community was not found.");

    public static readonly Error ForumThreadNotFound =
        Error.NotFound("Community.ForumThreadNotFound", "The specified thread was not found.");

    public static readonly Error StudentNotMember =
        Error.Forbidden("Community.StudentNotMember", "Only members of the community can publish in it.");

    public static Error DuplicateCommunityName(string name) =>
        Error.Conflict("Community.DuplicateCommunityName", $"A community named '{name}' already exists.");

    public static Error StudentAlreadyMember(Guid studentId) =>
        Error.Conflict("Community.StudentAlreadyMember", $"Student {studentId} is already a member.");
}
