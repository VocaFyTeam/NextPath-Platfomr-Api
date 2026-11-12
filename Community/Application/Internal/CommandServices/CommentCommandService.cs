using VocaFy.NextPath.Platform.Community.Application.CommandServices;
using VocaFy.NextPath.Platform.Community.Domain.Model;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.CommandServices;

public class CommentCommandService(
    ICommentRepository commentRepository,
    IForumThreadRepository forumThreadRepository,
    IUnitOfWork unitOfWork) : ICommentCommandService
{
    /// <summary>
    ///     Saves the comment and increments the thread counter in the same transaction,
    ///     so the frontend no longer needs to PATCH commentsCount.
    /// </summary>
    public async Task<Result<Comment>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var forumThread = await forumThreadRepository.FindByThreadIdAsync(command.ForumThreadId, cancellationToken);
        if (forumThread is null)
            return Result<Comment>.Failure(CommunityErrors.ForumThreadNotFound, CommunityError.ForumThreadNotFound);

        var comment = new Comment(command);
        forumThread.RegisterComment();
        await commentRepository.AddAsync(comment, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Comment>.Success(comment);
    }
}
