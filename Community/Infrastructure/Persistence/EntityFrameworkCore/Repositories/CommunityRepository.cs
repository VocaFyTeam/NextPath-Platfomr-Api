using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Community.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <remarks>Members is an owned collection, EF Core loads it automatically.</remarks>
public class CommunityRepository(AppDbContext context)
    : BaseRepository<InterestCommunity>(context), ICommunityRepository
{
    public async Task<InterestCommunity?> FindByCommunityIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var communityId = new CommunityId(id);
        return await Context.Set<InterestCommunity>()
            .FirstOrDefaultAsync(community => community.Id == communityId, cancellationToken);
    }

    public async Task<InterestCommunity?> FindByCommunityIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var communityId = new CommunityId(id);
        return await Context.Set<InterestCommunity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(community => community.Id == communityId, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await Context.Set<InterestCommunity>().AnyAsync(community => community.Name == name,
            cancellationToken);
    }
}
