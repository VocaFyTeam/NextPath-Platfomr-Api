using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NextPath.Platform.CareerExploration.Application.QueryServices;
using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model;
using NextPath.Platform.CareerExploration.Domain.Model.Errors;
using NextPath.Platform.CareerExploration.Domain.Model.Queries;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;
using NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;
using NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace NextPath.Platform.CareerExploration.Interfaces.Rest;

/// <summary>Career catalog: search, detail, comparison and compatibility (HU15-HU18, TS09, TS10).</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Careers")]
// TODO(Iam): add [Authorize] when Joaquin publishes the Iam authorization attribute.
public class CareersController(
    ICareerQueryService careerQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>
    ///     Lists careers. Filters (all optional): area, modality, durationYears, university (acronym), q (free text).
    ///     If one or more <c>id</c> values are sent (<c>?id=1&amp;id=2&amp;id=3</c>) it returns only those careers (comparison, max 3).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CareerResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCareers(
        [FromQuery] string? area,
        [FromQuery] string? modality,
        [FromQuery] int? durationYears,
        [FromQuery] string? university,
        [FromQuery] string? q,
        [FromQuery(Name = "id")] int[]? ids,
        CancellationToken cancellationToken)
    {
        IEnumerable<Career> careers;

        if (ids is { Length: > 0 })
        {
            var distinctIds = ids.Distinct().ToList();
            if (distinctIds.Count > CareerExplorationErrors.MaxCareersToCompare)
                return CareerExplorationActionResultAssembler.ToBadRequest(
                    this, problemDetailsFactory, CareerExplorationError.ComparisonLimitExceeded);

            careers = await careerQueryService.Handle(
                new GetCareersByIdsQuery(distinctIds.Select(id => new CareerId(id)).ToList()), cancellationToken);
        }
        else
        {
            careers = await careerQueryService.Handle(
                new GetAllCareersQuery(area, modality, durationYears, university, q), cancellationToken);
        }

        return Ok(careers.Select(CareerResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Full career sheet: curriculum info, universities, salaries, projection and recommendations.</summary>
    [HttpGet("{careerId:int}")]
    [ProducesResponseType(typeof(CareerResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCareerById(int careerId, CancellationToken cancellationToken)
    {
        var career = await careerQueryService.Handle(new GetCareerByIdQuery(new CareerId(careerId)),
            cancellationToken);
        if (career is null)
            return CareerExplorationActionResultAssembler.ToNotFound(
                this, problemDetailsFactory, CareerExplorationError.CareerNotFound);

        return Ok(CareerResourceFromEntityAssembler.ToResourceFromEntity(career));
    }

    /// <summary>Compatibility (0-100) between a student's vocational profile and a career (TS10).</summary>
    [HttpGet("{careerId:int}/compatibility")]
    [ProducesResponseType(typeof(CareerCompatibilityResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCareerCompatibility(
        int careerId,
        [FromQuery] int studentId,
        CancellationToken cancellationToken)
    {
        if (studentId <= 0)
            return CareerExplorationActionResultAssembler.ToBadRequest(
                this, problemDetailsFactory, CareerExplorationError.InvalidStudentId);

        var result = await careerQueryService.Handle(
            new GetCareerCompatibilityQuery(new CareerId(careerId), new StudentId(studentId)), cancellationToken);

        return CareerExplorationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            compatibility => Ok(new CareerCompatibilityResource(
                compatibility.CareerId,
                compatibility.StudentId,
                compatibility.Percentage,
                compatibility.Level.ToString().ToLowerInvariant())));
    }
}
