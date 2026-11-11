using System.Net.Mime;
using VocaFy.NextPath.Platform.VocationalResources.Application.CommandServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.QueryServices;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Queries;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest;

/// <summary>
///     Resource library of the psychologist (US09). Route /api/v1/resources, the one the web app already uses.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/resources")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vocational resources")]
public class VocationalResourcesController(
    IVocationalResourceCommandService commandService,
    IVocationalResourceQueryService queryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the resource library")]
    [EndpointDescription("Newest first. Optional filters: psychologistId and category (Guía, Lectura recomendada...)")]
    [ProducesResponseType(typeof(IEnumerable<VocationalResourceResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResources([FromQuery] Guid? psychologistId, [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var resources = await queryService.Handle(new GetAllVocationalResourcesQuery(psychologistId, category),
            cancellationToken);
        return Ok(resources.Select(VocationalResourceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{resourceId:guid}")]
    [EndpointSummary("Get a resource by id")]
    [ProducesResponseType(typeof(VocationalResourceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResourceById(Guid resourceId, CancellationToken cancellationToken)
    {
        var resource = await queryService.Handle(
            new GetVocationalResourceByIdQuery(new VocationalResourceId(resourceId)), cancellationToken);
        return resource is null
            ? VocationalResourcesActionResultAssembler.ToNotFound(this, errorLocalizer, problemDetailsFactory)
            : Ok(VocationalResourceResourceFromEntityAssembler.ToResourceFromEntity(resource));
    }

    [HttpPost]
    [EndpointSummary("Publish a resource")]
    [EndpointDescription("Only psychologists. The owner is the authenticated psychologist.")]
    [ProducesResponseType(typeof(VocationalResourceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateResource([FromBody] SaveVocationalResourceResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var command = VocationalResourceCommandFromResourceAssembler.ToCreateCommand(userId.Value, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return VocationalResourcesActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            created => CreatedAtAction(nameof(GetResourceById), new { resourceId = created.Id.Value },
                VocationalResourceResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPut("{resourceId:guid}")]
    [EndpointSummary("Update my resource")]
    [ProducesResponseType(typeof(VocationalResourceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateResource(Guid resourceId,
        [FromBody] SaveVocationalResourceResource resource, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var command = VocationalResourceCommandFromResourceAssembler.ToUpdateCommand(resourceId, userId.Value,
            resource);
        var result = await commandService.Handle(command, cancellationToken);
        return VocationalResourcesActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            updated => Ok(VocationalResourceResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpDelete("{resourceId:guid}")]
    [EndpointSummary("Delete my resource")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteResource(Guid resourceId, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var result = await commandService.Handle(new DeleteVocationalResourceCommand(resourceId, userId.Value),
            cancellationToken);
        return VocationalResourcesActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            _ => NoContent());
    }
}
