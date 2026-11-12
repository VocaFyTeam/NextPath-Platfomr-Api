using NextPath.Platform.Monitoring.Domain.Model;
using NextPath.Platform.Resources.Errors;
using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class MonitoringActionResultAssembler
{
    public static int ToStatusCodeFromMonitoringError(MonitoringError error)
    {
        return error switch
        {
            MonitoringError.HistoricalProfileNotFound => StatusCodes.Status404NotFound,
            MonitoringError.StudentGroupNotFound => StatusCodes.Status404NotFound,
            MonitoringError.VocationalReportNotFound => StatusCodes.Status404NotFound,
            MonitoringError.GroupComparisonReportNotFound => StatusCodes.Status404NotFound,
            MonitoringError.UserNotFound => StatusCodes.Status404NotFound,
            MonitoringError.PsychologistNotAuthorized => StatusCodes.Status403Forbidden,
            MonitoringError.HistoricalProfileAlreadyExists => StatusCodes.Status409Conflict,
            MonitoringError.DuplicateStudentGroupName => StatusCodes.Status409Conflict,
            MonitoringError.ReportAlreadyValidated => StatusCodes.Status409Conflict,
            MonitoringError.OperationCancelled => StatusCodes.Status409Conflict,
            MonitoringError.InvalidObservation => StatusCodes.Status400BadRequest,
            MonitoringError.InvalidStudentGroup => StatusCodes.Status400BadRequest,
            MonitoringError.InvalidReportContent => StatusCodes.Status400BadRequest,
            MonitoringError.InvalidFlag => StatusCodes.Status400BadRequest,
            MonitoringError.InvalidReportType => StatusCodes.Status400BadRequest,
            MonitoringError.AtLeastTwoGroupsRequired => StatusCodes.Status400BadRequest,
            MonitoringError.DatabaseError => StatusCodes.Status500InternalServerError,
            MonitoringError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest // Default
        };
    }
    public static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMonitoringError((MonitoringError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
    public static IActionResult ToActionResultFromGetOwnedResult<T>(
        ControllerBase controller,
        T? entity,
        Func<T, bool> isOwnedByAuthenticatedUser,
        MonitoringError notFoundError,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction) where T : class
    {
        if (entity is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromMonitoringError(notFoundError),
                notFoundError,
                errorLocalizer[notFoundError.ToString()]
            );

        if (!isOwnedByAuthenticatedUser(entity)) return ToForbiddenActionResult(controller, errorLocalizer, problemDetailsFactory);

        return successAction(entity);
    }
    public static IActionResult ToForbiddenActionResult(
        ControllerBase controller,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            controller,
            StatusCodes.Status403Forbidden,
            MonitoringError.PsychologistNotAuthorized,
            errorLocalizer[nameof(MonitoringError.PsychologistNotAuthorized)]
        );
    }
    public static IActionResult ToBadRequestActionResult(
        ControllerBase controller,
        MonitoringError error,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return problemDetailsFactory.CreateProblemDetails(
            controller,
            StatusCodes.Status400BadRequest,
            error,
            errorLocalizer[error.ToString()]
        );
    }
}
