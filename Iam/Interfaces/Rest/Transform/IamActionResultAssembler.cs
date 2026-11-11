using VocaFy.NextPath.Platform.Iam.Domain.Model;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
    public static int ToStatusCodeFromIamError(IamError error) => error switch
    {
        IamError.UserNotFound => StatusCodes.Status404NotFound,
        IamError.StudentNotFound => StatusCodes.Status404NotFound,
        IamError.PsychologistNotFound => StatusCodes.Status404NotFound,
        IamError.EmailAlreadyTaken => StatusCodes.Status409Conflict,
        IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
        IamError.InvalidRole => StatusCodes.Status400BadRequest,
        IamError.WeakPassword => StatusCodes.Status400BadRequest,
        IamError.MissingStudentData => StatusCodes.Status400BadRequest,
        IamError.MissingPsychologistData => StatusCodes.Status400BadRequest,
        IamError.InvalidLinkingCode => StatusCodes.Status400BadRequest,
        IamError.InvalidOrExpiredResetToken => StatusCodes.Status400BadRequest,
        IamError.ProfileOwnershipRequired => StatusCodes.Status403Forbidden,
        IamError.OperationCancelled => StatusCodes.Status409Conflict,
        IamError.DatabaseError => StatusCodes.Status500InternalServerError,
        IamError.InternalServerError => StatusCodes.Status500InternalServerError,
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

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>404 problem details for a query that found nothing.</summary>
    public static IActionResult ToNotFound(
        ControllerBase controller,
        IamError error,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromIamError(error), error,
            errorLocalizer[error.ToString()]);
    }
}
