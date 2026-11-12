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

/// <summary>
///     Forum threads (posts) of the communities: /api/v1/threads (same path the frontend already uses).
/// </summary>
[ApiController]
[Route("api/v1/threads")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Community Threads")]
public class ThreadsController(
    IForumThreadCommandService forumThreadCommandService,
    IForumThreadQueryService forumThreadQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get threads, newest first")]
    [EndpointDescription("With ?communityId={guid} returns only the threads of that community.")]
    [ProducesResponseType(typeof(IEnumerable<ThreadResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetThreads([FromQuery] Guid? communityId, CancellationToken cancellationToken)
    {
        IEnumerable<ForumThread> threads = communityId is null
            ? await forumThreadQueryService.Handle(new GetAllForumThreadsQuery(), cancellationToken)
            : await forumThreadQueryService.Handle(new GetForumThreadsByCommunityIdQuery(communityId.Value),
                cancellationToken);
        return Ok(threads.Select(ThreadResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{threadId:guid}")]
    [EndpointSummary("Get a thread by id")]
    [ProducesResponseType(typeof(ThreadResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThreadById(Guid threadId, CancellationToken cancellationToken)
    {
        var thread = await forumThreadQueryService.Handle(new GetForumThreadByIdQuery(threadId), cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromEntity(this, thread,
            CommunityError.ForumThreadNotFound, errorLocalizer,
            found => Ok(ThreadResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [EndpointSummary("Publish a thread in a community (US34)")]
    [ProducesResponseType(typeof(ThreadResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateThread([FromBody] CreateThreadResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateForumThreadCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await forumThreadCommandService.Handle(command, cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            created => CreatedAtAction(nameof(GetThreadById), new { threadId = created.Id.Value },
                ThreadResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPost("{threadId:guid}/likes")]
    [EndpointSummary("Like a thread (US35)")]
    [ProducesResponseType(typeof(ThreadResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LikeThread(Guid threadId, CancellationToken cancellationToken)
    {
        var result = await forumThreadCommandService.Handle(new LikeForumThreadCommand(threadId), cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            thread => Ok(ThreadResourceFromEntityAssembler.ToResourceFromEntity(thread)));
    }

    [HttpPost("{threadId:guid}/reports")]
    [EndpointSummary("Report a thread (US35)")]
    [ProducesResponseType(typeof(ThreadResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReportThread(Guid threadId, [FromBody] ReportThreadResource resource,
        CancellationToken cancellationToken)
    {
        var command = ReportForumThreadCommandFromResourceAssembler.ToCommandFromResource(threadId, resource);
        var result = await forumThreadCommandService.Handle(command, cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            thread => Ok(ThreadResourceFromEntityAssembler.ToResourceFromEntity(thread)));
    }
}
