using System.Net.Mime;
using VocaFy.NextPath.Platform.Assessments.Application.CommandServices;
using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest;

[ApiController]
[Route("api/v1/vocational-tests")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vocational Tests")]
public class VocationalTestsController(
    IVocationalTestCommandService vocationalTestCommandService,
    IVocationalTestQueryService vocationalTestQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all vocational tests")]
    [EndpointDescription("Returns the catalog of vocational tests ordered by the 'order' field.")]
    [ProducesResponseType(typeof(IEnumerable<VocationalTestResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVocationalTests(CancellationToken cancellationToken)
    {
        var tests = await vocationalTestQueryService.Handle(new GetAllVocationalTestsQuery(), cancellationToken);
        return Ok(tests.Select(VocationalTestResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{vocationalTestId:guid}")]
    [EndpointSummary("Get a vocational test by id")]
    [ProducesResponseType(typeof(VocationalTestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVocationalTestById(Guid vocationalTestId, CancellationToken cancellationToken)
    {
        var test = await vocationalTestQueryService.Handle(new GetVocationalTestByIdQuery(vocationalTestId),
            cancellationToken);
        return AssessmentsActionResultAssembler.ToActionResultFromEntity(this, test,
            AssessmentsError.VocationalTestNotFound, errorLocalizer,
            found => Ok(VocationalTestResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [EndpointSummary("Create a vocational test with its questions")]
    [EndpointDescription("Used to load the test catalog (NextPath IA, Holland, Kuder...).")]
    [ProducesResponseType(typeof(VocationalTestResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateVocationalTest([FromBody] CreateVocationalTestResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateVocationalTestCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await vocationalTestCommandService.Handle(command, cancellationToken);
        return AssessmentsActionResultAssembler.ToActionResultFromResult(this, result,
            created => CreatedAtAction(nameof(GetVocationalTestById), new { vocationalTestId = created.Id.Value },
                VocationalTestResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }
}
