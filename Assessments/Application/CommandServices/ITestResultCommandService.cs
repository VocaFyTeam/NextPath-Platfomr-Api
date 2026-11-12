using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Assessments.Application.CommandServices;

public interface ITestResultCommandService
{
    Task<Result<TestResult>> Handle(SubmitTestResultCommand command, CancellationToken cancellationToken);

    Task<Result<TestResult>> Handle(UpdateTestResultCommand command, CancellationToken cancellationToken);
}
