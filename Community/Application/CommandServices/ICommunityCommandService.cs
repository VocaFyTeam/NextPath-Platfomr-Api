using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Community.Application.CommandServices;

public interface ICommunityCommandService
{
    Task<Result<InterestCommunity>> Handle(CreateCommunityCommand command, CancellationToken cancellationToken);

    Task<Result<InterestCommunity>> Handle(JoinCommunityCommand command, CancellationToken cancellationToken);
}
