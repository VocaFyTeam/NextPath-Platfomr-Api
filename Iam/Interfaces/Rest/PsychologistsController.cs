using System.Net.Mime;
using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Psychologists")]
public class PsychologistsController(
    IPsychologistCommandService psychologistCommandService,
    IPsychologistQueryService psychologistQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all psychologists")]
    [ProducesResponseType(typeof(IEnumerable<PsychologistResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPsychologists(CancellationToken cancellationToken)
    {
        return Forbid();
    }

    [HttpGet("{psychologistId:guid}")]
    [EndpointSummary("Get a psychologist by id")]
    [ProducesResponseType(typeof(PsychologistResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPsychologistById(Guid psychologistId, CancellationToken cancellationToken)
    {
        var psychologist = await psychologistQueryService.Handle(
            new GetPsychologistByIdQuery(new PsychologistId(psychologistId)), cancellationToken);
        if (psychologist is null)
            return IamActionResultAssembler.ToNotFound(this, IamError.PsychologistNotFound, errorLocalizer,
                problemDetailsFactory);
        if (this.GetAuthenticatedUserId() != psychologist.UserId.Value) return Forbid();
        return Ok(PsychologistResourceFromEntityAssembler.ToResourceFromEntity(psychologist));
    }

    [HttpPut("{psychologistId:guid}")]
    [EndpointSummary("Update my psychologist profile")]
    [ProducesResponseType(typeof(PsychologistResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePsychologist(Guid psychologistId,
        [FromBody] UpdatePsychologistResource resource, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var command = IamCommandFromResourceAssembler.ToCommandFromResource(psychologistId, userId.Value, resource);
        var result = await psychologistCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            psychologist => Ok(PsychologistResourceFromEntityAssembler.ToResourceFromEntity(psychologist)));
    }

}
