using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;

public interface IStudentGroupCommandService
{
    Task<Result<StudentGroup>> Handle(CreateStudentGroupCommand command, CancellationToken cancellationToken);
}
