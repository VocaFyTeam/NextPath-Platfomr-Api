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

[ApiController]
[Route("api/v1")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Password reset")]
public class PasswordResetController(
    IPasswordResetCommandService passwordResetCommandService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost("password-reset-tokens")]
    [AllowAnonymous]
    [EndpointSummary("Request a password reset")]
    [EndpointDescription("Creates a reset token valid for 30 minutes and sends it to the email (in development it is written to the console)")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetResource resource,
        CancellationToken cancellationToken)
    {
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await passwordResetCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, _ => Accepted());
    }

    [HttpPost("password-resets")]
    [AllowAnonymous]
    [EndpointSummary("Reset the password")]
    [EndpointDescription("Uses a valid reset token to set a new password; the token is marked as used")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordResource resource,
        CancellationToken cancellationToken)
    {
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await passwordResetCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, _ => NoContent());
    }
}
