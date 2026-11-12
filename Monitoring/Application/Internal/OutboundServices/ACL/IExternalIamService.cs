namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;

/// <summary>
///     What Monitoring needs from IAM. Ids are the student_id / psychologist_id UUIDs.
/// </summary>
public interface IExternalIamService
{
    Task<bool> ExistsStudentByIdAsync(Guid studentId, CancellationToken cancellationToken);

    Task<bool> ExistsPsychologistByIdAsync(Guid psychologistId, CancellationToken cancellationToken);
}
