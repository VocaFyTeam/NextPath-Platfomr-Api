using System.Net.Mime;
using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest;

/// <summary>
///     GET /api/v1/test-questions?testId={guid} (same contract the frontend uses with json-server).
/// </summary>
[ApiController]
[Route("api/v1/test-questions")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vocational Tests")]
public class TestQuestionsController(IVocationalTestQueryService vocationalTestQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the questions of a vocational test")]
    [ProducesResponseType(typeof(IEnumerable<TestQuestionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestionsByTestId([FromQuery] Guid testId, CancellationToken cancellationToken)
    {
        var questions = await vocationalTestQueryService.Handle(
            new GetTestQuestionsByVocationalTestIdQuery(testId), cancellationToken);
        return Ok(questions.Select(TestQuestionResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
