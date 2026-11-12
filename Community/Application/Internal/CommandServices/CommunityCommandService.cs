using VocaFy.NextPath.Platform.Community.Application.CommandServices;
using VocaFy.NextPath.Platform.Community.Domain.Model;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Community.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Community.Application.Internal.CommandServices;

public class CommunityCommandService(ICommunityRepository communityRepository, IUnitOfWork unitOfWork)
    : ICommunityCommandService
{
    public async Task<Result<InterestCommunity>> Handle(CreateCommunityCommand command,
        CancellationToken cancellationToken)
    {
        if (await communityRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Result<InterestCommunity>.Failure(CommunityErrors.DuplicateCommunityName(command.Name),
                CommunityError.DuplicateCommunityName);

        var community = new InterestCommunity(command);
        await communityRepository.AddAsync(community, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<InterestCommunity>.Success(community);
    }

    public async Task<Result<InterestCommunity>> Handle(JoinCommunityCommand command,
        CancellationToken cancellationToken)
    {
        var community = await communityRepository.FindByCommunityIdAsync(command.CommunityId, cancellationToken);
        if (community is null)
            return Result<InterestCommunity>.Failure(CommunityErrors.CommunityNotFound,
                CommunityError.CommunityNotFound);

        if (community.HasMember(command.StudentId))
            return Result<InterestCommunity>.Failure(CommunityErrors.StudentAlreadyMember(command.StudentId),
                CommunityError.StudentAlreadyMember);

        community.Join(command.StudentId);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<InterestCommunity>.Success(community);
    }
}
