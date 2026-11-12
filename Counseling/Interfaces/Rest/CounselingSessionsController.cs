using System.Net.Mime;
using VocaFy.NextPath.Platform.Counseling.Application.CommandServices;
using VocaFy.NextPath.Platform.Counseling.Application.QueryServices;
using VocaFy.NextPath.Platform.Counseling.Domain.Model;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using EUserRole = VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects.EUserRole;

namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest;

/// <summary>
///     Orientation sessions between psychologists and students (US05, US13, US28).
///     Route /api/v1/counseling-sessions, the one the web app already uses.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/counseling-sessions")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Counseling sessions")]
public class CounselingSessionsController(
    ISessionCommandService commandService,
    ISessionQueryService queryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get my sessions")]
    [EndpointDescription(
        "Chronological order. Psychologists get the sessions they host and students the ones they are invited to. " +
        "Optional filters: psychologistId, studentId, groupId and status (SCHEDULED, COMPLETED, CANCELLED).")]
    [ProducesResponseType(typeof(IEnumerable<CounselingSessionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSessions([FromQuery] Guid? psychologistId, [FromQuery] Guid? studentId,
        [FromQuery] string? groupId, [FromQuery] string? status, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        ESessionStatus? requestedStatus = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!CounselingSessionFormat.TryParseStatus(status, out var parsedStatus))
                return ErrorResult(CounselingError.InvalidSessionStatus);
            requestedStatus = parsedStatus;
        }

        var sessions = await queryService.Handle(
            new GetAllSessionsQuery(userId.Value, psychologistId, studentId, groupId, requestedStatus),
            cancellationToken);
        return Ok(sessions.Select(CounselingSessionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{sessionId:guid}")]
    [EndpointSummary("Get a session by id")]
    [EndpointDescription("Only the host psychologist or an invited student can see it.")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionById(Guid sessionId, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var session = await queryService.Handle(new GetSessionByIdQuery(new SessionId(sessionId), userId.Value),
            cancellationToken);
        return session is null
            ? ErrorResult(CounselingError.SessionNotFound)
            : Ok(CounselingSessionResourceFromEntityAssembler.ToResourceFromEntity(session));
    }

    [HttpPost]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Schedule a session")]
    [EndpointDescription(
        "type INDIVIDUAL needs studentId; type GROUP needs 2 to 30 studentIds. The host is the authenticated " +
        "psychologist. Fails with 409 when the psychologist or a student already has a session at that time.")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ScheduleSession([FromBody] SaveCounselingSessionResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        if (!CounselingSessionFormat.TryParseType(resource.Type, out var type))
            return ErrorResult(CounselingError.InvalidSessionType);
        if (!CounselingSessionFormat.TryParseSchedule(resource.Date, resource.Time, out var scheduledDateTime))
            return ErrorResult(CounselingError.InvalidSessionSchedule);
        if (CounselingSessionCommandFromResourceAssembler.ToStudentIds(resource).Count == 0)
            return ErrorResult(CounselingError.InvalidParticipantsCount);

        var result = type == ESessionType.Group
            ? await commandService.Handle(
                CounselingSessionCommandFromResourceAssembler.ToScheduleGroupCommand(userId.Value, resource,
                    scheduledDateTime), cancellationToken)
            : await commandService.Handle(
                CounselingSessionCommandFromResourceAssembler.ToScheduleIndividualCommand(userId.Value, resource,
                    scheduledDateTime), cancellationToken);

        return CounselingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            created => CreatedAtAction(nameof(GetSessionById), new { sessionId = created.Id.Value },
                CounselingSessionResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPut("{sessionId:guid}")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Update or reschedule my session")]
    [EndpointDescription(
        "Same body as POST; the type cannot change. Sending status CANCELLED or COMPLETED only applies that " +
        "change (and the notes).")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSession(Guid sessionId, [FromBody] SaveCounselingSessionResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        if (!CounselingSessionFormat.TryParseType(resource.Type, out var type))
            return ErrorResult(CounselingError.InvalidSessionType);
        if (!CounselingSessionFormat.TryParseSchedule(resource.Date, resource.Time, out var scheduledDateTime))
            return ErrorResult(CounselingError.InvalidSessionSchedule);

        ESessionStatus? status = null;
        if (!string.IsNullOrWhiteSpace(resource.Status))
        {
            if (!CounselingSessionFormat.TryParseStatus(resource.Status, out var parsedStatus))
                return ErrorResult(CounselingError.InvalidSessionStatus);
            status = parsedStatus;
        }

        var command = CounselingSessionCommandFromResourceAssembler.ToUpdateCommand(sessionId, userId.Value,
            resource, type, scheduledDateTime, status);
        return ToSessionResult(await commandService.Handle(command, cancellationToken));
    }

    [HttpPatch("{sessionId:guid}/cancel")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Cancel my session")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        return ToSessionResult(await commandService.Handle(new CancelSessionCommand(sessionId, userId.Value),
            cancellationToken));
    }

    [HttpPatch("{sessionId:guid}/complete")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Mark my session as completed")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompleteSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        return ToSessionResult(await commandService.Handle(new CompleteSessionCommand(sessionId, userId.Value),
            cancellationToken));
    }

    [HttpPatch("{sessionId:guid}/notes")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Save the notes of my session")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveNotes(Guid sessionId, [FromBody] SaveSessionNotesResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        return ToSessionResult(await commandService.Handle(
            new SaveSessionNotesCommand(sessionId, userId.Value, resource.Notes), cancellationToken));
    }

    [HttpPost("{sessionId:guid}/participants")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Add a student to my group session")]
    [ProducesResponseType(typeof(CounselingSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddParticipant(Guid sessionId, [FromBody] AddSessionParticipantResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        return ToSessionResult(await commandService.Handle(
            new AddSessionParticipantCommand(sessionId, userId.Value, resource.StudentId), cancellationToken));
    }

    [HttpPost("grouping-suggestions")]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Suggest optimal groups for a group session")]
    [EndpointDescription(
        "Groups students that share their dominant vocational area, in groups of at most maxParticipants. " +
        "Needs at least two students.")]
    [ProducesResponseType(typeof(IEnumerable<GroupingSuggestionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SuggestGrouping([FromBody] SuggestGroupingResource resource)
    {
        var result = queryService.Handle(
            CounselingSessionCommandFromResourceAssembler.ToSuggestGroupingQuery(resource));
        return CounselingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            suggestions => Ok(suggestions.Select(CounselingSessionResourceFromEntityAssembler.ToResourceFromValueObject)));
    }

    private IActionResult ToSessionResult(Result<Session> result) =>
        CounselingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            session => Ok(CounselingSessionResourceFromEntityAssembler.ToResourceFromEntity(session)));

    private IActionResult ErrorResult(CounselingError error) =>
        CounselingActionResultAssembler.ToProblem(this, error, errorLocalizer, problemDetailsFactory);
}
