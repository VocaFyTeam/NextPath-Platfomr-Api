using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;

public class ProblemDetailsFactory(
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer,
    Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory aspNetCoreProblemDetailsFactory)
{
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        Enum? errorEnum,
        string detailMessage)
    {
        var title = errorEnum != null ? errorLocalizer[$"{errorEnum}"] : commonLocalizer["InternalServerError"];
        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            title,
            detail: detailMessage,
            instance: controller.HttpContext.Request.Path);
        return controller.StatusCode(statusCode, problemDetails);
    }

    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        string titleKey,
        string detailKey,
        params object[] detailArgs)
    {
        var problemDetails = aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            commonLocalizer[titleKey],
            detail: errorLocalizer[detailKey, detailArgs],
            instance: controller.HttpContext.Request.Path);
        return controller.StatusCode(statusCode, problemDetails);
    }
}
