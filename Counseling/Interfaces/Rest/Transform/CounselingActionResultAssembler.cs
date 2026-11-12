using VocaFy.NextPath.Platform.Counseling.Domain.Model;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Transform;

public static class CounselingActionResultAssembler
{
    public static int ToStatusCode(CounselingError error) => error switch
    {
        CounselingError.SessionNotFound => StatusCodes.Status404NotFound,
        CounselingError.StudentNotFound => StatusCodes.Status404NotFound,
        CounselingError.PsychologistRoleRequired => StatusCodes.Status403Forbidden,
        CounselingError.NotSessionHost => StatusCodes.Status403Forbidden,
        CounselingError.InvalidSessionData => StatusCodes.Status400BadRequest,
        CounselingError.InvalidSessionType => StatusCodes.Status400BadRequest,
        CounselingError.InvalidSessionStatus => StatusCodes.Status400BadRequest,
        CounselingError.InvalidSessionSchedule => StatusCodes.Status400BadRequest,
        CounselingError.InvalidSessionDuration => StatusCodes.Status400BadRequest,
        CounselingError.SessionDateInPast => StatusCodes.Status400BadRequest,
        CounselingError.SessionTypeCannotChange => StatusCodes.Status400BadRequest,
        CounselingError.NotAGroupSession => StatusCodes.Status400BadRequest,
        CounselingError.InvalidParticipantsCount => StatusCodes.Status400BadRequest,
        CounselingError.InsufficientProfilesForGrouping => StatusCodes.Status400BadRequest,
        CounselingError.SchedulingConflictDetected => StatusCodes.Status409Conflict,
        CounselingError.SessionNotScheduled => StatusCodes.Status409Conflict,
        CounselingError.GroupSessionFull => StatusCodes.Status409Conflict,
        CounselingError.StudentAlreadyParticipant => StatusCodes.Status409Conflict,
        CounselingError.OperationCancelled => StatusCodes.Status409Conflict,
        CounselingError.DatabaseError => StatusCodes.Status500InternalServerError,
        CounselingError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>Works for Result&lt;T&gt; and for Result (which is Result&lt;object&gt;).</summary>
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCode((CounselingError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>Problem details for an error detected before reaching the application layer (or a 404).</summary>
    public static IActionResult ToProblem(
        ControllerBase controller,
        CounselingError error,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(error), error,
            errorLocalizer[error.ToString()]);
    }
}
