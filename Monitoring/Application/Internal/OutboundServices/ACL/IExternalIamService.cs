namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;


public interface IExternalIamService
{

    Task<bool> ExistsUserByIdAsync(int userId, CancellationToken cancellationToken);
}
