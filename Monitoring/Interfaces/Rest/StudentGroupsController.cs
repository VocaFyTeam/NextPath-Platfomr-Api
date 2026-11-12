using System.Net.Mime;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Student Monitoring - Student groups")]
public class StudentGroupsController(
    IStudentGroupCommandService groupCommandService,
    IStudentGroupQueryService groupQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get the groups of the authenticated psychologist")]
    [EndpointDescription("Get the groups (for example Grupo A and Grupo B) of the authenticated psychologist.")]
    [ProducesResponseType(typeof(IEnumerable<StudentGroupResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllStudentGroups([FromQuery] Guid? psychologistId,
        CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();
        if (psychologistId.HasValue && psychologistId.Value != authenticatedId.Value)
            return MonitoringActionResultAssembler.ToForbiddenActionResult(this, _errorLocalizer,
                _problemDetailsFactory);

        var groups = await groupQueryService.Handle(new GetAllStudentGroupsQuery(authenticatedId.Value),
            cancellationToken);
        return Ok(groups.Select(StudentGroupResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a group by its id")]
    [EndpointDescription("Get a group by its id")]
    [ProducesResponseType(typeof(StudentGroupResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentGroupById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var group = await groupQueryService.Handle(new GetStudentGroupByIdQuery(id), cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            group,
            found => found.IsOwnedBy(authenticatedId.Value),
            MonitoringError.StudentGroupNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(StudentGroupResourceFromEntityAssembler.ToResourceFromEntity(found))
        );
    }

    [HttpPost]
    [EndpointSummary("Create a group")]
    [EndpointDescription("Creates a group of students owned by the authenticated psychologist.")]
    [ProducesResponseType(typeof(StudentGroupResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateStudentGroup([FromBody] CreateStudentGroupResource resource,
        CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var command =
            CreateStudentGroupCommandFromResourceAssembler.ToCommandFromResource(resource, authenticatedId.Value);
        var result = await groupCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            created => CreatedAtAction(nameof(GetStudentGroupById), new { id = created.Id.Value },
                StudentGroupResourceFromEntityAssembler.ToResourceFromEntity(created))
        );
    }
}
