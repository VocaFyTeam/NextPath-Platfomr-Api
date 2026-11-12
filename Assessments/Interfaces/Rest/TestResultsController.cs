using System.Net.Mime;
using VocaFy.NextPath.Platform.Assessments.Application.CommandServices;
using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest;

[ApiController]
[Route("api/v1/test-results")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Test Results")]
public class TestResultsController(
    ITestResultCommandService testResultCommandService,
    ITestResultQueryService testResultQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get test results")]
    [EndpointDescription("With ?studentId={guid} returns the results of one student; without it returns all results.")]
    [ProducesResponseType(typeof(IEnumerable<TestResultResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestResults([FromQuery] Guid? studentId, CancellationToken cancellationToken)
    {
        IEnumerable<TestResult> results = studentId is null
            ? await testResultQueryService.Handle(new GetAllTestResultsQuery(), cancellationToken)
            : await testResultQueryService.Handle(new GetTestResultsByStudentIdQuery(studentId.Value),
                cancellationToken);
        return Ok(results.Select(TestResultResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{testResultId:guid}")]
    [EndpointSummary("Get a test result by id")]
    [ProducesResponseType(typeof(TestResultResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestResultById(Guid testResultId, CancellationToken cancellationToken)
    {
        var testResult = await testResultQueryService.Handle(new GetTestResultByIdQuery(testResultId),
            cancellationToken);
        return AssessmentsActionResultAssembler.ToActionResultFromEntity(this, testResult,
            AssessmentsError.TestResultNotFound, errorLocalizer,
            found => Ok(TestResultResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [EndpointSummary("Submit a vocational test")]
    [EndpointDescription("status 'completed' calculates the vocational profile; 'pending' only saves answers.")]
    [ProducesResponseType(typeof(TestResultResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitTestResult([FromBody] SubmitTestResultResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubmitTestResultCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await testResultCommandService.Handle(command, cancellationToken);
        return AssessmentsActionResultAssembler.ToActionResultFromResult(this, result,
            created => CreatedAtAction(nameof(GetTestResultById), new { testResultId = created.Id.Value },
                TestResultResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPut("{testResultId:guid}")]
    [EndpointSummary("Continue or finish a pending test")]
    [ProducesResponseType(typeof(TestResultResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTestResult(Guid testResultId, [FromBody] SubmitTestResultResource resource,
        CancellationToken cancellationToken)
    {
        var command = SubmitTestResultCommandFromResourceAssembler.ToUpdateCommandFromResource(testResultId, resource);
        var result = await testResultCommandService.Handle(command, cancellationToken);
        return AssessmentsActionResultAssembler.ToActionResultFromResult(this, result,
            updated => Ok(TestResultResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }
}
