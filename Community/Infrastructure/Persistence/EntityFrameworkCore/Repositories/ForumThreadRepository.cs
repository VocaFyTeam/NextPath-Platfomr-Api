using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Community.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ForumThreadRepository(AppDbContext context)
    : BaseRepository<ForumThread>(context), IForumThreadRepository
{
    public async Task<ForumThread?> FindByThreadIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var threadId = new ForumThreadId(id);
        return await Context.Set<ForumThread>()
            .FirstOrDefaultAsync(thread => thread.Id == threadId, cancellationToken);
    }

    public async Task<ForumThread?> FindByThreadIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        var threadId = new ForumThreadId(id);
        return await Context.Set<ForumThread>()
            .AsNoTracking()
            .FirstOrDefaultAsync(thread => thread.Id == threadId, cancellationToken);
    }

    public async Task<IEnumerable<ForumThread>> ListNewestFirstAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<ForumThread>()
            .AsNoTracking()
            .OrderByDescending(thread => thread.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ForumThread>> FindByCommunityIdAsNoTrackingAsync(Guid communityId,
        CancellationToken cancellationToken)
    {
        var id = new CommunityId(communityId);
        return await Context.Set<ForumThread>()
            .AsNoTracking()
            .Where(thread => thread.CommunityId == id)
            .OrderByDescending(thread => thread.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
