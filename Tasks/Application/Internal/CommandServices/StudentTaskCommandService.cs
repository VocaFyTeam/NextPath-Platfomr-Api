using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Shared.Domain.Repositories;
using NextPath.Platform.Tasks.Application.CommandServices;
using NextPath.Platform.Tasks.Domain.Model;
using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.Commands;
using NextPath.Platform.Tasks.Domain.Model.Errors;
using NextPath.Platform.Tasks.Domain.Repositories;

namespace NextPath.Platform.Tasks.Application.Internal.CommandServices;

public class StudentTaskCommandService(
    IStudentTaskRepository studentTaskRepository,
    IUnitOfWork unitOfWork)
    : IStudentTaskCommandService
{
    public async Task<Result<StudentTask>> Handle(CreateStudentTaskCommand command,
        CancellationToken cancellationToken)
    {
        var order = command.Order
                    ?? await studentTaskRepository.GetNextOrderByStudentIdAsync(command.StudentId, cancellationToken);

        var task = new StudentTask(command with { Order = order });
        await studentTaskRepository.AddAsync(task, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<StudentTask>.Success(task);
    }

    public async Task<Result<StudentTask>> Handle(ChangeStudentTaskStatusCommand command,
        CancellationToken cancellationToken)
    {
        var task = await studentTaskRepository.FindByStudentTaskIdAsync(command.StudentTaskId, cancellationToken);
        if (task is null)
            return Result<StudentTask>.Failure(TasksErrors.StudentTaskNotFound, TasksError.StudentTaskNotFound);

        task.ChangeStatus(command.Status);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<StudentTask>.Success(task);
    }

    public async Task<Result<StudentTask>> Handle(UpdateStudentTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await studentTaskRepository.FindByStudentTaskIdAsync(command.StudentTaskId, cancellationToken);
        if (task is null)
            return Result<StudentTask>.Failure(TasksErrors.StudentTaskNotFound, TasksError.StudentTaskNotFound);

        task.Update(command.Title, command.DueDate, command.Order, command.Status);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<StudentTask>.Success(task);
    }
}
