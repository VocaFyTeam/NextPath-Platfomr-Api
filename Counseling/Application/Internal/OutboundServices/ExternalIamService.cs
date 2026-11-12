using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Counseling.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service: the only class of this context that talks to IAM (through its ACL facade)
///     and translates the answer into this context's value objects.
/// </summary>
public class ExternalIamService(IIamContextFacade iamContextFacade)
{
    /// <returns>The PsychologistId of the user, or null if the user is not a psychologist.</returns>
    public async Task<PsychologistId?> FetchPsychologistIdByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var psychologistId = await iamContextFacade.FetchPsychologistIdByUserId(userId, cancellationToken);
        return psychologistId == Guid.Empty ? null : new PsychologistId(psychologistId);
    }

    /// <returns>The StudentId of the user, or null if the user is not a student.</returns>
    public async Task<StudentId?> FetchStudentIdByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var studentId = await iamContextFacade.FetchStudentIdByUserId(userId, cancellationToken);
        return studentId == Guid.Empty ? null : new StudentId(studentId);
    }

    public async Task<bool> ExistsStudent(StudentId studentId, CancellationToken cancellationToken) =>
        await iamContextFacade.ExistsStudentById(studentId.Value, cancellationToken);
}
