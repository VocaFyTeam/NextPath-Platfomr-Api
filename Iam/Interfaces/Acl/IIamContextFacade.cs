namespace VocaFy.NextPath.Platform.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<Guid> FetchPsychologistIdByUserId(Guid userId, CancellationToken cancellationToken);

    Task<Guid> FetchStudentIdByUserId(Guid userId, CancellationToken cancellationToken);

    Task<bool> ExistsPsychologistById(Guid psychologistId, CancellationToken cancellationToken);

    Task<bool> ExistsStudentById(Guid studentId, CancellationToken cancellationToken);

    Task<string> FetchEmailByUserId(Guid userId, CancellationToken cancellationToken);
}
