using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;


public class ExternalIamService(IIamContextFacade iamContextFacade) : IExternalIamService
{

    public async Task<bool> ExistsUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var username = await iamContextFacade.FetchUsernameByUserId(userId, cancellationToken);
        return !string.IsNullOrEmpty(username);
    }
}
