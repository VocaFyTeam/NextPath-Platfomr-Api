using VocaFy.NextPath.Platform.Assessments.Domain.Model;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

/// <summary>
///     Turns Result&lt;T&gt; (commands) or nullable entities (queries) into HTTP responses with ProblemDetails.
/// </summary>
public static class AssessmentsActionResultAssembler
{
    private static int ToStatusCode(AssessmentsError error) => error switch
    {
        AssessmentsError.VocationalTestNotFound => StatusCodes.Status404NotFound,
        AssessmentsError.TestResultNotFound => StatusCodes.Status404NotFound,
        AssessmentsError.DuplicateVocationalTestTitle => StatusCodes.Status409Conflict,
        AssessmentsError.TestResultAlreadyCompleted => StatusCodes.Status409Conflict,
        AssessmentsError.InvalidAnswers => StatusCodes.Status400BadRequest,
        AssessmentsError.OperationCancelled => StatusCodes.Status409Conflict,
        AssessmentsError.DatabaseError => StatusCodes.Status500InternalServerError,
        AssessmentsError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var error = result.Error is AssessmentsError assessmentsError
            ? assessmentsError
            : AssessmentsError.InternalServerError;
        return controller.Problem(detail: result.Message, statusCode: ToStatusCode(error),
            title: error.ToString());
    }

    public static IActionResult ToActionResultFromEntity<T>(
        ControllerBase controller,
        T? entity,
        AssessmentsError notFoundError,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        Func<T, IActionResult> successAction) where T : class
    {
        if (entity is not null) return successAction(entity);

        return controller.Problem(detail: errorLocalizer[notFoundError.ToString()],
            statusCode: ToStatusCode(notFoundError), title: notFoundError.ToString());
    }
}
