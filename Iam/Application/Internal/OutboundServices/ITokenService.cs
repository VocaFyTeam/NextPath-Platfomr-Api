using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);

    Task<Guid?> ValidateToken(string token);
}
