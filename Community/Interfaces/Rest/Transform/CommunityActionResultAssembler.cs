using VocaFy.NextPath.Platform.Community.Domain.Model;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CommunityActionResultAssembler
{
    private static int ToStatusCode(CommunityError error) => error switch
    {
        CommunityError.CommunityNotFound => StatusCodes.Status404NotFound,
        CommunityError.ForumThreadNotFound => StatusCodes.Status404NotFound,
        CommunityError.DuplicateCommunityName => StatusCodes.Status409Conflict,
        CommunityError.StudentAlreadyMember => StatusCodes.Status409Conflict,
        CommunityError.StudentNotMember => StatusCodes.Status403Forbidden,
        CommunityError.OperationCancelled => StatusCodes.Status409Conflict,
        CommunityError.DatabaseError => StatusCodes.Status500InternalServerError,
        CommunityError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResultFromResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var error = result.Error is CommunityError communityError
            ? communityError
            : CommunityError.InternalServerError;
        return controller.Problem(detail: result.Message, statusCode: ToStatusCode(error),
            title: error.ToString());
    }

    public static IActionResult ToActionResultFromEntity<T>(
        ControllerBase controller,
        T? entity,
        CommunityError notFoundError,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        Func<T, IActionResult> successAction) where T : class
    {
        if (entity is not null) return successAction(entity);

        return controller.Problem(detail: errorLocalizer[notFoundError.ToString()],
            statusCode: ToStatusCode(notFoundError), title: notFoundError.ToString());
    }
}
