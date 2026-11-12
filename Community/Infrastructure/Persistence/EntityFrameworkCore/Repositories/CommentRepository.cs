using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Community.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CommentRepository(AppDbContext context) : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> FindByThreadIdAsNoTrackingAsync(Guid forumThreadId,
        CancellationToken cancellationToken)
    {
        var threadId = new ForumThreadId(forumThreadId);
        return await Context.Set<Comment>()
            .AsNoTracking()
            .Where(comment => comment.ForumThreadId == threadId)
            .OrderBy(comment => comment.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
