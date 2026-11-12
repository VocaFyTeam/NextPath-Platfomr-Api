using VocaFy.NextPath.Platform.Assessments.Application.CommandServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Application.Internal.CommandServices;

public class VocationalTestCommandService(
    IVocationalTestRepository vocationalTestRepository,
    IUnitOfWork unitOfWork) : IVocationalTestCommandService
{
    public async Task<Result<VocationalTest>> Handle(CreateVocationalTestCommand command,
        CancellationToken cancellationToken)
    {
        if (await vocationalTestRepository.ExistsByTitleAsync(command.Title, cancellationToken))
            return Result<VocationalTest>.Failure(AssessmentsErrors.DuplicateVocationalTestTitle(command.Title),
                AssessmentsError.DuplicateVocationalTestTitle);

        var vocationalTest = new VocationalTest(command);
        await vocationalTestRepository.AddAsync(vocationalTest, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalTest>.Success(vocationalTest);
    }
}
