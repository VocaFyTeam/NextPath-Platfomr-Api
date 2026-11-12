using System.Net.Mime;
using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
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
[Tags("Students")]
public class StudentsController(
    IStudentCommandService studentCommandService,
    IStudentQueryService studentQueryService,
    IPsychologistQueryService psychologistQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [Authorize(EUserRole.Psychologist)]
    [EndpointSummary("Get students")]
    [EndpointDescription("All students, or only the ones linked to a psychologist code (?linkedPsychologistCode=PSY-XXXXXX)")]
    [ProducesResponseType(typeof(IEnumerable<StudentResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudents([FromQuery] string? linkedPsychologistCode,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();
        var psychologist = await psychologistQueryService.Handle(
            new GetPsychologistByUserIdQuery(new UserId(userId.Value)), cancellationToken);
        if (psychologist is null) return StatusCode(StatusCodes.Status403Forbidden);
        var requestedCode = Psychologist.NormalizeLinkingCode(linkedPsychologistCode);
        if (requestedCode is not null && requestedCode != psychologist.UniqueLinkingCode) return StatusCode(StatusCodes.Status403Forbidden);
        var students = await studentQueryService.Handle(new GetAllStudentsQuery(psychologist.UniqueLinkingCode),
            cancellationToken);
        return Ok(students.Select(StudentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{studentId:guid}")]
    [EndpointSummary("Get a student by id")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentById(Guid studentId, CancellationToken cancellationToken)
    {
        var student = await studentQueryService.Handle(new GetStudentByIdQuery(new StudentId(studentId)),
            cancellationToken);
        if (student is null)
            return IamActionResultAssembler.ToNotFound(this, IamError.StudentNotFound, errorLocalizer, problemDetailsFactory);

        if (HttpContext.Items["User"] is not User user) return Unauthorized();
        var isOwner = user.IsStudent() && student.UserId == user.Id;
        var isLinkedPsychologist = false;
        if (user.IsPsychologist())
        {
            var psychologist = await psychologistQueryService.Handle(new GetPsychologistByUserIdQuery(user.Id), cancellationToken);
            isLinkedPsychologist = psychologist is not null &&
                                   student.LinkedPsychologistCode == psychologist.UniqueLinkingCode;
        }
        if (!isOwner && !isLinkedPsychologist) return StatusCode(StatusCodes.Status403Forbidden);
        return Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(student));
    }

    [HttpPut("{studentId:guid}")]
    [EndpointSummary("Update my student profile")]
    [EndpointDescription("Only the owner can update it. Also links or unlinks the psychologist code.")]
    [ProducesResponseType(typeof(StudentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudent(Guid studentId, [FromBody] UpdateStudentResource resource,
        CancellationToken cancellationToken)
    {
        var userId = this.GetAuthenticatedUserId();
        if (userId is null) return Unauthorized();

        var command = IamCommandFromResourceAssembler.ToCommandFromResource(studentId, userId.Value, resource);
        var result = await studentCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            student => Ok(StudentResourceFromEntityAssembler.ToResourceFromEntity(student)));
    }
}
