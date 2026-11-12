using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.QueryServices;

public class StudentQueryService(IStudentRepository studentRepository) : IStudentQueryService
{
    public async Task<Student?> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken) =>
        await studentRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);

    public async Task<Student?> Handle(GetStudentByUserIdQuery query, CancellationToken cancellationToken) =>
        await studentRepository.FindByUserIdAsync(query.UserId, cancellationToken);

    public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.LinkedPsychologistCode))
            return await studentRepository.ListAsNoTrackingAsync(cancellationToken);

        var code = Psychologist.NormalizeLinkingCode(query.LinkedPsychologistCode)!;
        return await studentRepository.FindByLinkedPsychologistCodeAsync(code, cancellationToken);
    }
}
