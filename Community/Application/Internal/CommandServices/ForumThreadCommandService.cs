using VocaFy.NextPath.Platform.Community.Application.CommandServices;
using VocaFy.NextPath.Platform.Community.Domain.Model;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.CommandServices;

public class ForumThreadCommandService(
    IForumThreadRepository forumThreadRepository,
    ICommunityRepository communityRepository,
    IUnitOfWork unitOfWork) : IForumThreadCommandService
{
    /// <summary>US34: only members of the community can publish.</summary>
    public async Task<Result<ForumThread>> Handle(CreateForumThreadCommand command,
        CancellationToken cancellationToken)
    {
        var community =
            await communityRepository.FindByCommunityIdAsNoTrackingAsync(command.CommunityId, cancellationToken);
        if (community is null)
            return Result<ForumThread>.Failure(CommunityErrors.CommunityNotFound, CommunityError.CommunityNotFound);

        if (!community.HasMember(command.AuthorId))
            return Result<ForumThread>.Failure(CommunityErrors.StudentNotMember, CommunityError.StudentNotMember);

        var forumThread = new ForumThread(command);
        await forumThreadRepository.AddAsync(forumThread, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ForumThread>.Success(forumThread);
    }

    public async Task<Result<ForumThread>> Handle(LikeForumThreadCommand command, CancellationToken cancellationToken)
    {
        var forumThread = await forumThreadRepository.FindByThreadIdAsync(command.ForumThreadId, cancellationToken);
        if (forumThread is null)
            return Result<ForumThread>.Failure(CommunityErrors.ForumThreadNotFound,
                CommunityError.ForumThreadNotFound);

        forumThread.Like();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ForumThread>.Success(forumThread);
    }

    public async Task<Result<ForumThread>> Handle(ReportForumThreadCommand command,
        CancellationToken cancellationToken)
    {
        var forumThread = await forumThreadRepository.FindByThreadIdAsync(command.ForumThreadId, cancellationToken);
        if (forumThread is null)
            return Result<ForumThread>.Failure(CommunityErrors.ForumThreadNotFound,
                CommunityError.ForumThreadNotFound);

        forumThread.Report(command.Reason);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ForumThread>.Success(forumThread);
    }
}
