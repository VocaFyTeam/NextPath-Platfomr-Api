using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken);
    Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken);
}
