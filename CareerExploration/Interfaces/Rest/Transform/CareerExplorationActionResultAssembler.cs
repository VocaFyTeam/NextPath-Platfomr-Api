using Microsoft.AspNetCore.Mvc;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;

public static class CareerExplorationActionResultAssembler
{
    /// <summary>The HTTP status comes from the error type (NotFound, Conflict, Validation...).</summary>
    private static int ToStatusCode(EErrorType type)
    {
        return type switch
        {
            EErrorType.NotFound => StatusCodes.Status404NotFound,
            EErrorType.Conflict => StatusCodes.Status409Conflict,
            EErrorType.Validation => StatusCodes.Status400BadRequest,
            EErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            EErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    /// <summary>Success runs <paramref name="successAction"/>; failure becomes a ProblemDetails response.</summary>
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return ToProblem(controller, result, problemDetailsFactory);
    }

    public static IActionResult ToProblem<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            controller, ToStatusCode(result.DomainError.Type), result.Error, result.Message);
    }

    public static IActionResult ToNotFound(
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory,
        CareerExplorationError error)
    {
        var domainError = error switch
        {
            CareerExplorationError.FavoriteCareerNotFound => CareerExplorationErrors.FavoriteCareerNotFound,
            _ => CareerExplorationErrors.CareerNotFound
        };
        return problemDetailsFactory.CreateProblemDetails(
            controller, StatusCodes.Status404NotFound, error, domainError.Message);
    }

    public static IActionResult ToBadRequest(
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory,
        CareerExplorationError error)
    {
        var domainError = error switch
        {
            CareerExplorationError.ComparisonLimitExceeded => CareerExplorationErrors.ComparisonLimitExceeded,
            _ => CareerExplorationErrors.InvalidStudentId
        };
        return problemDetailsFactory.CreateProblemDetails(
            controller, StatusCodes.Status400BadRequest, error, domainError.Message);
    }
}
