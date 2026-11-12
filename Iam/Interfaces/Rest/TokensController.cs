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
using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Authentication")]
public class TokensController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Sign in")]
    [EndpointDescription("Validates email and password, activates the session and returns a JWT")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateToken([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            session => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                session.user, session.profileId, session.token)));
    }

    [HttpDelete]
    [EndpointSummary("Sign out")]
    [EndpointDescription("Closes the session of the authenticated user; its token stops working")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeToken(CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var result = await userCommandService.Handle(new SignOutCommand(userId.Value), cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, _ => NoContent());
    }
}
