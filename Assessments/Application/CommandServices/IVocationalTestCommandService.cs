using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Assessments.Application.CommandServices;

public interface IVocationalTestCommandService
{
    Task<Result<VocationalTest>> Handle(CreateVocationalTestCommand command, CancellationToken cancellationToken);
}
