namespace VocaFy.NextPath.Platform.Community.Domain.Model;

public enum CommunityError
{
    None,
    CommunityNotFound,
    ForumThreadNotFound,
    DuplicateCommunityName,
    StudentAlreadyMember,
    StudentNotMember,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
