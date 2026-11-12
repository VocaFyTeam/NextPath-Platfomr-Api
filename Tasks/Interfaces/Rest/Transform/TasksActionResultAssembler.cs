using Microsoft.AspNetCore.Mvc;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using VocaFy.NextPath.Platform.Tasks.Domain.Model;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Transform;

public static class TasksActionResultAssembler
{
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

    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        return problemDetailsFactory.CreateProblemDetails(
            controller, ToStatusCode(result.DomainError.Type), result.Error, result.Message);
    }

    public static IActionResult ToNotFound(ControllerBase controller, ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            controller, StatusCodes.Status404NotFound, TasksError.StudentTaskNotFound,
            TasksErrors.StudentTaskNotFound.Message);
    }

    public static IActionResult ToBadRequest(
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory,
        TasksError error)
    {
        var domainError = error == TasksError.InvalidTaskStatus
            ? TasksErrors.InvalidTaskStatus
            : TasksErrors.InvalidStudentId;
        return problemDetailsFactory.CreateProblemDetails(
            controller, StatusCodes.Status400BadRequest, error, domainError.Message);
    }
}
