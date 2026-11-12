using NextPath.Platform.Monitoring.Application.CommandServices;
using NextPath.Platform.Monitoring.Domain.Model;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Errors;
using NextPath.Platform.Monitoring.Domain.Repositories;
using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.CommandServices;

public class StudentGroupCommandService(
    IStudentGroupRepository groupRepository,
    IUnitOfWork unitOfWork)
    : IStudentGroupCommandService
{
    public async Task<Result<StudentGroup>> Handle(CreateStudentGroupCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Name?.Trim() ?? string.Empty;
        if (name.Length == 0 || name.Length > StudentGroup.MaxNameLength ||
            (command.Description?.Trim().Length ?? 0) > StudentGroup.MaxDescriptionLength)
            return Result<StudentGroup>.Failure(MonitoringErrors.InvalidStudentGroup,
                MonitoringError.InvalidStudentGroup);

        if (await groupRepository.ExistsByNameAndPsychologistIdAsync(name, command.PsychologistId, cancellationToken))
            return Result<StudentGroup>.Failure(MonitoringErrors.DuplicateStudentGroupNameWithName(name),
                MonitoringError.DuplicateStudentGroupName);

        var group = new StudentGroup(command);
        await groupRepository.AddAsync(group, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<StudentGroup>.Success(group);
    }
}
