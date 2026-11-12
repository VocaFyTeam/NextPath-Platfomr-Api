using System.Net.Mime;
using VocaFy.NextPath.Platform.Community.Application.CommandServices;
using VocaFy.NextPath.Platform.Community.Application.QueryServices;
using VocaFy.NextPath.Platform.Community.Domain.Model;
using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest;

[ApiController]
[Route("api/v1/communities")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Communities")]
public class CommunitiesController(
    ICommunityCommandService communityCommandService,
    ICommunityQueryService communityQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all interest communities")]
    [ProducesResponseType(typeof(IEnumerable<CommunityResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCommunities(CancellationToken cancellationToken)
    {
        var communities = await communityQueryService.Handle(new GetAllCommunitiesQuery(), cancellationToken);
        return Ok(communities.Select(CommunityResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{communityId:guid}")]
    [EndpointSummary("Get a community by id")]
    [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCommunityById(Guid communityId, CancellationToken cancellationToken)
    {
        var community = await communityQueryService.Handle(new GetCommunityByIdQuery(communityId), cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromEntity(this, community,
            CommunityError.CommunityNotFound, errorLocalizer,
            found => Ok(CommunityResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [EndpointSummary("Create an interest community")]
    [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateCommunityCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await communityCommandService.Handle(command, cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            created => CreatedAtAction(nameof(GetCommunityById), new { communityId = created.Id.Value },
                CommunityResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPost("{communityId:guid}/members")]
    [EndpointSummary("Join a community (US33)")]
    [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> JoinCommunity(Guid communityId, [FromBody] JoinCommunityResource resource,
        CancellationToken cancellationToken)
    {
        var command = JoinCommunityCommandFromResourceAssembler.ToCommandFromResource(communityId, resource);
        var result = await communityCommandService.Handle(command, cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            community => Ok(CommunityResourceFromEntityAssembler.ToResourceFromEntity(community)));
    }
}
