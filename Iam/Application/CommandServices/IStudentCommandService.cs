using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Iam.Application.CommandServices;

public interface IStudentCommandService
{
    Task<Result<Student>> Handle(UpdateStudentCommand command, CancellationToken cancellationToken);
}
