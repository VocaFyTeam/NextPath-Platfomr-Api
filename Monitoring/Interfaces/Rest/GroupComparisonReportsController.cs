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
[Tags("Reports - Group comparison reports")]
public class GroupComparisonReportsController(
    IGroupComparisonReportCommandService reportCommandService,
    IGroupComparisonReportQueryService reportQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [EndpointSummary("Get the group comparison reports of the authenticated psychologist")]
    [EndpointDescription("Get the group comparison reports of the authenticated psychologist.")]
    [ProducesResponseType(typeof(IEnumerable<GroupComparisonReportResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGroupComparisonReports(CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var reports = await reportQueryService.Handle(new GetAllGroupComparisonReportsQuery(authenticatedId.Value),
            cancellationToken);
        return Ok(reports.Select(GroupComparisonReportResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a group comparison report by its id")]
    [EndpointDescription("Get a group comparison report by its id")]
    [ProducesResponseType(typeof(GroupComparisonReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupComparisonReportById([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var report = await reportQueryService.Handle(new GetGroupComparisonReportByIdQuery(id), cancellationToken);
        return MonitoringActionResultAssembler.ToActionResultFromGetOwnedResult(
            this,
            report,
            found => found.IsOwnedBy(authenticatedId.Value),
            MonitoringError.GroupComparisonReportNotFound,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(GroupComparisonReportResourceFromEntityAssembler.ToResourceFromEntity(found))
        );
    }

    [HttpPost]
    [EndpointSummary("Generate a comparison between groups")]
    [EndpointDescription(
        "Compares two or more groups of the authenticated psychologist and returns the aggregated conclusions.")]
    [ProducesResponseType(typeof(GroupComparisonReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateGroupComparisonReport(
        [FromBody] GenerateGroupComparisonReportResource resource, CancellationToken cancellationToken)
    {
        var authenticatedId = await AuthenticatedUserAssembler.GetAuthenticatedPsychologistIdAsync(HttpContext);
        if (authenticatedId is null) return Unauthorized();

        var command =
            GenerateGroupComparisonReportCommandFromResourceAssembler.ToCommandFromResource(resource,
                authenticatedId.Value);
        var result = await reportCommandService.Handle(command, cancellationToken);

        return MonitoringActionResultAssembler.ToActionResultFromResult(
            this,
            result,
            _problemDetailsFactory,
            generated => CreatedAtAction(nameof(GetGroupComparisonReportById), new { id = generated.Id.Value },
                GroupComparisonReportResourceFromEntityAssembler.ToResourceFromEntity(generated))
        );
    }
}
