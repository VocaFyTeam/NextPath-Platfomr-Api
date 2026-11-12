using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using NextPath.Platform.Tasks.Application.CommandServices;
using NextPath.Platform.Tasks.Application.QueryServices;
using NextPath.Platform.Tasks.Domain.Model;
using NextPath.Platform.Tasks.Domain.Model.Commands;
using NextPath.Platform.Tasks.Domain.Model.Queries;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;
using NextPath.Platform.Tasks.Interfaces.Rest.Resources;
using NextPath.Platform.Tasks.Interfaces.Rest.Transform;

namespace NextPath.Platform.Tasks.Interfaces.Rest;

/// <summary>Personal vocational plan and progress of the student (HU21-HU24, TS12).</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Tasks")]
// TODO(Iam): add [Authorize] and read the student id from the token instead of the query/body.
public class TasksController(
    IStudentTaskCommandService studentTaskCommandService,
    IStudentTaskQueryService studentTaskQueryService,
    IVocationalProgressQueryService vocationalProgressQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Tasks of a student ordered by <c>order</c>. Extra params such as <c>_sort</c> are ignored.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentTaskResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTasksByStudentId([FromQuery] int studentId,
        CancellationToken cancellationToken)
    {
        if (studentId <= 0)
            return TasksActionResultAssembler.ToBadRequest(this, problemDetailsFactory, TasksError.InvalidStudentId);

        var tasks = await studentTaskQueryService.Handle(
            new GetStudentTasksByStudentIdQuery(new StudentId(studentId)), cancellationToken);
        return Ok(tasks.Select(StudentTaskResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Overall progress: 50 % tests, 30 % tasks, 20 % favorite careers (HU21).</summary>
    [HttpGet("progress")]
    [ProducesResponseType(typeof(VocationalProgressResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVocationalProgress([FromQuery] int studentId,
        CancellationToken cancellationToken)
    {
        if (studentId <= 0)
            return TasksActionResultAssembler.ToBadRequest(this, problemDetailsFactory, TasksError.InvalidStudentId);

        var progress = await vocationalProgressQueryService.Handle(
            new GetVocationalProgressByStudentIdQuery(new StudentId(studentId)), cancellationToken);
        return Ok(VocationalProgressResourceFromValueObjectAssembler.ToResource(progress));
    }

    [HttpGet("{taskId:int}")]
    [ProducesResponseType(typeof(StudentTaskResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskById(int taskId, CancellationToken cancellationToken)
    {
        var task = await studentTaskQueryService.Handle(
            new GetStudentTaskByIdQuery(new StudentTaskId(taskId)), cancellationToken);
        if (task is null) return TasksActionResultAssembler.ToNotFound(this, problemDetailsFactory);

        return Ok(StudentTaskResourceFromEntityAssembler.ToResourceFromEntity(task));
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentTaskResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateStudentTaskResource resource,
        CancellationToken cancellationToken)
    {
        var command = new CreateStudentTaskCommand(
            new StudentId(resource.StudentId), resource.Title, resource.DueDate, resource.Order);
        var result = await studentTaskCommandService.Handle(command, cancellationToken);

        return TasksActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            task => CreatedAtAction(nameof(GetTaskById), new { taskId = (int)task.Id },
                StudentTaskResourceFromEntityAssembler.ToResourceFromEntity(task)));
    }

    /// <summary>Replaces title, due date, order and status of a task.</summary>
    [HttpPut("{taskId:int}")]
    [ProducesResponseType(typeof(StudentTaskResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateStudentTaskResource resource,
        CancellationToken cancellationToken)
    {
        if (!TaskStatusMapper.TryParse(resource.Status, out var status))
            return TasksActionResultAssembler.ToBadRequest(this, problemDetailsFactory, TasksError.InvalidTaskStatus);

        var command = new UpdateStudentTaskCommand(
            new StudentTaskId(taskId), resource.Title, resource.DueDate, resource.Order, status);
        var result = await studentTaskCommandService.Handle(command, cancellationToken);

        return TasksActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            task => Ok(StudentTaskResourceFromEntityAssembler.ToResourceFromEntity(task)));
    }

    /// <summary>Marks a task as "pending" or "completed". This is what the frontend calls when the student ticks a task.</summary>
    [HttpPatch("{taskId:int}")]
    [ProducesResponseType(typeof(StudentTaskResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeTaskStatus(int taskId, [FromBody] ChangeStudentTaskStatusResource resource,
        CancellationToken cancellationToken)
    {
        if (!TaskStatusMapper.TryParse(resource.Status, out var status))
            return TasksActionResultAssembler.ToBadRequest(this, problemDetailsFactory, TasksError.InvalidTaskStatus);

        var result = await studentTaskCommandService.Handle(
            new ChangeStudentTaskStatusCommand(new StudentTaskId(taskId), status), cancellationToken);

        return TasksActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            task => Ok(StudentTaskResourceFromEntityAssembler.ToResourceFromEntity(task)));
    }
}
