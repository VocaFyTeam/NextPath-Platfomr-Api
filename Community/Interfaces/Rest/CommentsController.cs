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
[Route("api/v1/comments")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Community Threads")]
public class CommentsController(
    ICommentCommandService commentCommandService,
    ICommentQueryService commentQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the comments of a thread")]
    [EndpointDescription("GET /api/v1/comments?threadId={guid}, oldest first.")]
    [ProducesResponseType(typeof(IEnumerable<CommentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommentsByThreadId([FromQuery] Guid threadId,
        CancellationToken cancellationToken)
    {
        var comments = await commentQueryService.Handle(new GetCommentsByForumThreadIdQuery(threadId),
            cancellationToken);
        return Ok(comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Comment on a thread (US34)")]
    [EndpointDescription("Also increments commentsCount of the thread.")]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commentCommandService.Handle(command, cancellationToken);
        return CommunityActionResultAssembler.ToActionResultFromResult(this, result,
            created => StatusCode(StatusCodes.Status201Created,
                CommentResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }
}
