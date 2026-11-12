using System.Net.Mime;
using NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using NextPath.Platform.Resources.Errors;
using NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NextPath.Platform.Monitoring.Application.CommandServices;
using NextPath.Platform.Monitoring.Application.QueryServices;
using NextPath.Platform.Monitoring.Domain.Model;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Queries;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;
using NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

namespace NextPath.Platform.Monitoring.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Reports - Vocational reports")]
public class VocationalReportsController(
    IVocationalReportCommandService reportCommandService,
    IVocationalReportQueryService reportQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get the vocational reports of the authenticated psychologist")]
    [EndpointDescription("Get the vocational reports of the authenticated psychologist, optionally filtered by student.")]
    [ProducesResponseType(typeof(IEnumerable<VocationalReportResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVocationalReports([FromQuery] int? studentId,
        CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var reports = await reportQueryService.Handle(new GetAllVocationalReportsQuery(authenticatedId.Value, studentId),
            cancellationToken);
        return Ok(reports.Select(VocationalReportResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a vocational report by its id")]
    [EndpointDescription("Get a vocational report by its id")]
    [ProducesResponseType(typeof(VocationalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVocationalReportById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var report = await reportQueryService.Handle(new GetVocationalReportByIdQuery(id), cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            report,
            found => found.IsOwnedBy(authenticatedId.Value),
            MonitoringError.VocationalReportNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(VocationalReportResourceFromEntityAssembler.ToResourceFromEntity(found))
        );
    }

    [HttpPost]
    [EndpointSummary("Generate a vocational report")]
    [EndpointDescription(
        "Generates the report (type test, individualEvolution or psychological) of a student assigned to the authenticated psychologist. The report starts as not validated.")]
    [ProducesResponseType(typeof(VocationalReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateVocationalReport([FromBody] GenerateVocationalReportResource resource,
        CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        if (!EnumResourceMapper.TryParse<EReportType>(resource.ReportType, out var reportType))
            return MonitoringActionResultAssembler.ToBadRequestActionResult(this, MonitoringError.InvalidReportType,
                _errorLocalizer, _problemDetailsFactory);

        var command = GenerateVocationalReportCommandFromResourceAssembler.ToCommandFromResource(resource, reportType,
            authenticatedId.Value);
        var result = await reportCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            generated => CreatedAtAction(nameof(GetVocationalReportById), new { id = generated.Id.Value },
                VocationalReportResourceFromEntityAssembler.ToResourceFromEntity(generated))
        );
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Edit the content of a vocational report")]
    [EndpointDescription("Edits the synthesis of a report that has not been validated yet.")]
    [ProducesResponseType(typeof(VocationalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateVocationalReportContent([FromRoute] int id,
        [FromBody] UpdateVocationalReportContentResource resource, CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var command = UpdateVocationalReportContentCommandFromResourceAssembler.ToCommandFromResource(resource, id,
            authenticatedId.Value);
        var result = await reportCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            updated => Ok(VocationalReportResourceFromEntityAssembler.ToResourceFromEntity(updated))
        );
    }

    [HttpPost("{id:int}/validations")]
    [EndpointSummary("Validate a vocational report")]
    [EndpointDescription("The psychologist validates the report. A validated report cannot be modified.")]
    [ProducesResponseType(typeof(VocationalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateVocationalReport([FromRoute] int id, CancellationToken cancellationToken)
    {
        var authenticatedId = AuthenticatedUserAssembler.GetAuthenticatedUserId(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var result = await reportCommandService.Handle(new ValidateVocationalReportCommand(id, authenticatedId.Value),
            cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            validated => Ok(VocationalReportResourceFromEntityAssembler.ToResourceFromEntity(validated))
        );
    }
}
