using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application;
using VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IStudentRepository studentRepository,
    IPsychologistRepository psychologistRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    public async Task<Result<(User user, Guid profileId)>> Handle(SignUpCommand command,
        CancellationToken cancellationToken)
    {
        if (!PasswordPolicy.IsValid(command.Password))
            return Result<(User, Guid)>.Failure(IamErrors.WeakPassword, IamError.WeakPassword);

        if (!Enum.TryParse<EUserRole>(command.Role, true, out var role) || !Enum.IsDefined(role))
            return Result<(User, Guid)>.Failure(IamErrors.InvalidRole, IamError.InvalidRole);

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<(User, Guid)>.Failure(IamErrors.EmailAlreadyTaken, IamError.EmailAlreadyTaken);

        var user = new User(command.Email, hashingService.HashPassword(command.Password), role);
        Guid profileId;

        if (role == EUserRole.Student)
        {
            if (string.IsNullOrWhiteSpace(command.SchoolOrUniversity) || string.IsNullOrWhiteSpace(command.CurrentCycle))
                return Result<(User, Guid)>.Failure(IamErrors.MissingStudentData, IamError.MissingStudentData);

            var linkingCode = Psychologist.NormalizeLinkingCode(command.LinkedPsychologistCode);
            if (linkingCode is not null &&
                !await psychologistRepository.ExistsByUniqueLinkingCodeAsync(linkingCode, cancellationToken))
                return Result<(User, Guid)>.Failure(IamErrors.InvalidLinkingCode, IamError.InvalidLinkingCode);

            var student = new Student(user.Id, command.SchoolOrUniversity, command.CurrentCycle, linkingCode);
            await studentRepository.AddAsync(student, cancellationToken);
            profileId = student.Id.Value;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(command.ProfessionalLicense) || string.IsNullOrWhiteSpace(command.Specialty))
                return Result<(User, Guid)>.Failure(IamErrors.MissingPsychologistData,
                    IamError.MissingPsychologistData);

            var linkingCode = await GenerateUniqueLinkingCodeAsync(cancellationToken);
            var psychologist = new Psychologist(user.Id, command.ProfessionalLicense, command.Specialty, linkingCode);
            await psychologistRepository.AddAsync(psychologist, cancellationToken);
            profileId = psychologist.Id.Value;
        }

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<(User, Guid)>.Success((user, profileId));
    }

    public async Task<Result<(User user, Guid profileId, string token)>> Handle(SignInCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User, Guid, string)>.Failure(IamErrors.InvalidCredentials, IamError.InvalidCredentials);

        var profileId = user.IsStudent()
            ? (await studentRepository.FindByUserIdAsync(user.Id, cancellationToken))?.Id.Value ?? Guid.Empty
            : (await psychologistRepository.FindByUserIdAsync(user.Id, cancellationToken))?.Id.Value ?? Guid.Empty;

        user.StartSession();
        await unitOfWork.CompleteAsync(cancellationToken);

        var token = tokenService.GenerateToken(user);
        return Result<(User, Guid, string)>.Success((user, profileId, token));
    }

    public async Task<Result> Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(new UserId(command.UserId), cancellationToken);
        if (user is null) return Result.Failure(IamErrors.UserNotFound, IamError.UserNotFound);

        user.EndSession();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    private async Task<string> GenerateUniqueLinkingCodeAsync(CancellationToken cancellationToken)
    {
        string code;
        do
        {
            code = Psychologist.GenerateLinkingCode();
        } while (await psychologistRepository.ExistsByUniqueLinkingCodeAsync(code, cancellationToken));

        return code;
    }
}
