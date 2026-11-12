using VocaFy.NextPath.Platform.Community.Application.QueryServices;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.QueryServices;

public class CommunityQueryService(ICommunityRepository communityRepository) : ICommunityQueryService
{
    public async Task<IEnumerable<InterestCommunity>> Handle(GetAllCommunitiesQuery query,
        CancellationToken cancellationToken)
    {
        return await communityRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<InterestCommunity?> Handle(GetCommunityByIdQuery query, CancellationToken cancellationToken)
    {
        return await communityRepository.FindByCommunityIdAsNoTrackingAsync(query.CommunityId, cancellationToken);
    }
}
