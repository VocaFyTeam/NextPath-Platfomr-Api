using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.Commands;

namespace NextPath.Platform.Tasks.Application.CommandServices;

public interface IStudentTaskCommandService
{
    Task<Result<StudentTask>> Handle(CreateStudentTaskCommand command, CancellationToken cancellationToken);

    Task<Result<StudentTask>> Handle(ChangeStudentTaskStatusCommand command, CancellationToken cancellationToken);

    Task<Result<StudentTask>> Handle(UpdateStudentTaskCommand command, CancellationToken cancellationToken);
}
