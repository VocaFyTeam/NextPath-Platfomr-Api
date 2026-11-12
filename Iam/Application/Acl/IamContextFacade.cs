using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Iam.Application.Acl;

public class IamContextFacade(
    IUserQueryService userQueryService,
    IStudentQueryService studentQueryService,
    IPsychologistQueryService psychologistQueryService)
    : IIamContextFacade
{
    public async Task<Guid> FetchPsychologistIdByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var psychologist = await psychologistQueryService.Handle(
            new GetPsychologistByUserIdQuery(new UserId(userId)), cancellationToken);
        return psychologist?.Id.Value ?? Guid.Empty;
    }

    public async Task<Guid> FetchStudentIdByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var student = await studentQueryService.Handle(new GetStudentByUserIdQuery(new UserId(userId)),
            cancellationToken);
        return student?.Id.Value ?? Guid.Empty;
    }

    public async Task<bool> ExistsPsychologistById(Guid psychologistId, CancellationToken cancellationToken)
    {
        var psychologist = await psychologistQueryService.Handle(
            new GetPsychologistByIdQuery(new PsychologistId(psychologistId)), cancellationToken);
        return psychologist is not null;
    }

    public async Task<bool> ExistsStudentById(Guid studentId, CancellationToken cancellationToken)
    {
        var student = await studentQueryService.Handle(new GetStudentByIdQuery(new StudentId(studentId)),
            cancellationToken);
        return student is not null;
    }

    public async Task<string> FetchEmailByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(new UserId(userId)), cancellationToken);
        return user?.Email ?? string.Empty;
    }
}
