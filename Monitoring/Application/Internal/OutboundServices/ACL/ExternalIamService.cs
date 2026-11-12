using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;

public class ExternalIamService(IIamContextFacade iamContextFacade) : IExternalIamService
{
    public async Task<bool> ExistsStudentByIdAsync(Guid studentId, CancellationToken cancellationToken) =>
        await iamContextFacade.ExistsStudentById(studentId, cancellationToken);

    public async Task<bool> ExistsPsychologistByIdAsync(Guid psychologistId, CancellationToken cancellationToken) =>
        await iamContextFacade.ExistsPsychologistById(psychologistId, cancellationToken);
}
