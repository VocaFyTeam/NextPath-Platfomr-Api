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

public class PsychologistCommandService(
    IPsychologistRepository psychologistRepository,
    IUnitOfWork unitOfWork)
    : IPsychologistCommandService
{
    public async Task<Result<Psychologist>> Handle(UpdatePsychologistCommand command,
        CancellationToken cancellationToken)
    {
        var psychologist = await psychologistRepository.FindByIdAsync(new PsychologistId(command.PsychologistId),
            cancellationToken);
        if (psychologist is null)
            return Result<Psychologist>.Failure(IamErrors.PsychologistNotFound, IamError.PsychologistNotFound);

        if (psychologist.UserId.Value != command.RequesterUserId)
            return Result<Psychologist>.Failure(IamErrors.ProfileOwnershipRequired,
                IamError.ProfileOwnershipRequired);

        psychologist.UpdateProfile(command.ProfessionalLicense, command.Specialty);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Psychologist>.Success(psychologist);
    }
}
