using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Community.Application.QueryServices;

public interface ICommunityQueryService
{
    Task<IEnumerable<InterestCommunity>> Handle(GetAllCommunitiesQuery query, CancellationToken cancellationToken);

    Task<InterestCommunity?> Handle(GetCommunityByIdQuery query, CancellationToken cancellationToken);
}
