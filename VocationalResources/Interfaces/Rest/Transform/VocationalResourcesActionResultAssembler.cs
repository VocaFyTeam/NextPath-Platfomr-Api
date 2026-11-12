using VocaFy.NextPath.Platform.VocationalResources.Domain.Model;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Transform;

public static class VocationalResourcesActionResultAssembler
{
    public static int ToStatusCode(VocationalResourcesError error) => error switch
    {
        VocationalResourcesError.VocationalResourceNotFound => StatusCodes.Status404NotFound,
        VocationalResourcesError.NotAPsychologist => StatusCodes.Status403Forbidden,
        VocationalResourcesError.NotResourceOwner => StatusCodes.Status403Forbidden,
        VocationalResourcesError.DuplicateResourceTitle => StatusCodes.Status409Conflict,
        VocationalResourcesError.OperationCancelled => StatusCodes.Status409Conflict,
        VocationalResourcesError.DatabaseError => StatusCodes.Status500InternalServerError,
        VocationalResourcesError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCode((VocationalResourcesError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToNotFound(
        ControllerBase controller,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        const VocationalResourcesError error = VocationalResourcesError.VocationalResourceNotFound;
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(error), error,
            errorLocalizer[error.ToString()]);
    }
}
