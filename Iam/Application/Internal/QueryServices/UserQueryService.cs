using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken) =>
        await userRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);

    public async Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken) =>
        await userRepository.FindByEmailAsNoTrackingAsync(query.Email, cancellationToken);

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken) =>
        await userRepository.ListAsNoTrackingAsync(cancellationToken);
}
