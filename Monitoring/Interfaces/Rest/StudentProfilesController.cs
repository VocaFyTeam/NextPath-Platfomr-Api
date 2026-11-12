using System.Net.Mime;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Student Monitoring - Student profiles")]
public class StudentProfilesController(
    IHistoricalProfileCommandService profileCommandService,
    IHistoricalProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get the student profiles of the authenticated psychologist")]
    [EndpointDescription(
        "Lists the student profiles (flags, observation summary, group and last activity) of the students assigned to the authenticated psychologist. It can be filtered by group and student.")]
    [ProducesResponseType(typeof(IEnumerable<StudentProfileResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllStudentProfiles(
        [FromQuery] Guid? psychologistId,
        [FromQuery] int? groupId,
        [FromQuery] Guid? studentId,
        CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();
        if (psychologistId.HasValue && psychologistId.Value != authenticatedId.Value)
            return MonitoringActionResultAssembler.ToForbiddenActionResult(this, _errorLocalizer,
                _problemDetailsFactory);

        var query = new GetAllHistoricalProfilesQuery(authenticatedId.Value, groupId, studentId);
        var profiles = await profileQueryService.Handle(query, cancellationToken);
        return Ok(profiles.Select(StudentProfileResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a student profile by its id")]
    [EndpointDescription("Get a student profile by its id")]
    [ProducesResponseType(typeof(StudentProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentProfileById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var profile = await profileQueryService.Handle(new GetHistoricalProfileByIdQuery(id), cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            profile,
            found => found.IsAssignedTo(authenticatedId.Value),
            MonitoringError.HistoricalProfileNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(StudentProfileResourceFromEntityAssembler.ToResourceFromEntity(found))
        );
    }

    [HttpPost]
    [EndpointSummary("Open the profile of a student")]
    [EndpointDescription(
        "Assigns a student to the authenticated psychologist and opens its historical profile. A student has a single profile.")]
    [ProducesResponseType(typeof(StudentProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateStudentProfile([FromBody] CreateStudentProfileResource resource,
        CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var command =
            CreateHistoricalProfileCommandFromResourceAssembler.ToCommandFromResource(resource, authenticatedId.Value);
        var result = await profileCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            created => CreatedAtAction(nameof(GetStudentProfileById), new { id = created.Id.Value },
                StudentProfileResourceFromEntityAssembler.ToResourceFromEntity(created))
        );
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Update a student profile")]
    [EndpointDescription(
        "Updates the observation summary, the flags (inactivity, inconsistent, lowParticipation) and/or the group of a student profile. Omitted members stay unchanged.")]
    [ProducesResponseType(typeof(StudentProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudentProfile([FromRoute] int id,
        [FromBody] UpdateStudentProfileResource resource, CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        IReadOnlyCollection<EStudentFlag>? flags = null;
        if (resource.Flags is not null)
        {
            if (!EnumResourceMapper.TryParseMany<EStudentFlag>(resource.Flags, out var parsedFlags))
                return MonitoringActionResultAssembler.ToBadRequestActionResult(this, MonitoringError.InvalidFlag,
                    _errorLocalizer, _problemDetailsFactory);
            flags = parsedFlags;
        }

        var command = UpdateHistoricalProfileCommandFromResourceAssembler.ToCommandFromResource(resource, id,
            authenticatedId.Value, flags);
        var result = await profileCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            updated => Ok(StudentProfileResourceFromEntityAssembler.ToResourceFromEntity(updated))
        );
    }

    [HttpPost("inactivity-evaluations")]
    [EndpointSummary("Re-evaluate the inactivity of the students")]
    [EndpointDescription(
        "Flags with 'inactivity' the students of the authenticated psychologist that have more than 30 days without activity, and clears the flag of the others.")]
    [ProducesResponseType(typeof(IEnumerable<StudentProfileResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> EvaluateInactivity(CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var result = await profileCommandService.Handle(
            new EvaluateInactivityCommand(authenticatedId.Value), cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            profiles => Ok(profiles.Select(StudentProfileResourceFromEntityAssembler.ToResourceFromEntity))
        );
    }

    [HttpGet("{id:int}/recommendations")]
    [EndpointSummary("Get the automatic intervention recommendations of a student")]
    [EndpointDescription(
        "Suggests interventions (for example scheduleOneOnOneSession or assignExplorationWorkshop) according to the flags of the student.")]
    [ProducesResponseType(typeof(IEnumerable<InterventionRecommendationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecommendations([FromRoute] int id, CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var profile = await profileQueryService.Handle(new GetHistoricalProfileByIdQuery(id), cancellationToken);
        if (profile is not null && !profile.IsAssignedTo(authenticatedId.Value))
            return MonitoringActionResultAssembler.ToForbiddenActionResult(this, _errorLocalizer,
                _problemDetailsFactory);

        var recommendations = await profileQueryService.Handle(
            new GetInterventionRecommendationsByHistoricalProfileIdQuery(id), cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            recommendations,
            _ => true,
            MonitoringError.HistoricalProfileNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(found.Select(InterventionRecommendationResourceFromValueObjectAssembler
                .ToResourceFromValueObject))
        );
    }
}
