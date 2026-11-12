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
[Tags("Users")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IStudentQueryService studentQueryService,
    IPsychologistQueryService psychologistQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Sign up")]
    [EndpointDescription("Registers a student or a psychologist (users + students/psychologists)")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken cancellationToken)
    {
        var command = IamCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            created => CreatedAtAction(nameof(GetUserById), new { userId = created.user.Id.Value },
                UserResourceFromEntityAssembler.ToResourceFromEntity(created.user, created.profileId)));
    }

    [HttpGet("{userId:guid}")]
    [EndpointSummary("Get a user by id")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        if (this.GetAuthenticatedUserId() != userId) return Forbid();

        var user = await userQueryService.Handle(new GetUserByIdQuery(new UserId(userId)), cancellationToken);
        if (user is null)
            return IamActionResultAssembler.ToNotFound(this, IamError.UserNotFound, errorLocalizer,
                problemDetailsFactory);

        var profileId = user.IsStudent()
            ? (await studentQueryService.Handle(new GetStudentByUserIdQuery(user.Id), cancellationToken))?.Id.Value
            : (await psychologistQueryService.Handle(new GetPsychologistByUserIdQuery(user.Id), cancellationToken))
            ?.Id.Value;
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user, profileId ?? Guid.Empty));
    }

    [HttpGet]
    [EndpointSummary("Get all users")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        return Forbid();
    }
}
