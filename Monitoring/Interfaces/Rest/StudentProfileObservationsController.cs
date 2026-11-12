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
[Route("api/v1/student-profiles/{profileId:int}/observations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Student Monitoring - Professional observations")]
public class StudentProfileObservationsController(
    IHistoricalProfileCommandService profileCommandService,
    IHistoricalProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get the professional observations of a student")]
    [EndpointDescription("Get the professional observations of a student, newest first.")]
    [ProducesResponseType(typeof(IEnumerable<ProfessionalObservationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObservationsByProfileId([FromRoute] int profileId,
        CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var profile = await profileQueryService.Handle(new GetHistoricalProfileByIdQuery(profileId),
            cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            profile,
            found => found.IsAssignedTo(authenticatedId.Value),
            MonitoringError.HistoricalProfileNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(found.Observations
                .OrderByDescending(observation => observation.CreatedAt)
                .Select(ProfessionalObservationResourceFromEntityAssembler.ToResourceFromEntity))
        );
    }

    [HttpPost]
    [EndpointSummary("Record a professional observation")]
    [EndpointDescription(
        "Records a permanent note (with timestamp and digital signature of the psychologist) in the file of the student. Only the assigned psychologist can record it; otherwise 403.")]
    [ProducesResponseType(typeof(ProfessionalObservationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordObservation([FromRoute] int profileId,
        [FromBody] CreateProfessionalObservationResource resource, CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var command = RecordProfessionalObservationCommandFromResourceAssembler.ToCommandFromResource(resource,
            profileId, authenticatedId.Value);
        var result = await profileCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            recorded => CreatedAtAction(nameof(GetObservationsByProfileId), new { profileId },
                ProfessionalObservationResourceFromEntityAssembler.ToResourceFromEntity(recorded))
        );
    }
}
