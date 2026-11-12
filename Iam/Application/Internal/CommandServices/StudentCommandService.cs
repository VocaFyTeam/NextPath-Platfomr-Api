using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.CommandServices;

public class StudentCommandService(
    IStudentRepository studentRepository,
    IPsychologistRepository psychologistRepository,
    IUnitOfWork unitOfWork)
    : IStudentCommandService
{
    public async Task<Result<Student>> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
    {
        var student = await studentRepository.FindByIdAsync(new StudentId(command.StudentId), cancellationToken);
        if (student is null) return Result<Student>.Failure(IamErrors.StudentNotFound, IamError.StudentNotFound);

        if (student.UserId.Value != command.RequesterUserId)
            return Result<Student>.Failure(IamErrors.ProfileOwnershipRequired, IamError.ProfileOwnershipRequired);

        var linkingCode = Psychologist.NormalizeLinkingCode(command.LinkedPsychologistCode);
        if (linkingCode is not null &&
            !await psychologistRepository.ExistsByUniqueLinkingCodeAsync(linkingCode, cancellationToken))
            return Result<Student>.Failure(IamErrors.InvalidLinkingCode, IamError.InvalidLinkingCode);

        student.UpdateProfile(command.SchoolOrUniversity, command.CurrentCycle).LinkToPsychologist(linkingCode);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Student>.Success(student);
    }
}
