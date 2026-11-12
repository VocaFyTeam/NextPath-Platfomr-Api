using NextPath.Platform.Shared.Domain.Model.Errors;

namespace NextPath.Platform.CareerExploration.Domain.Model.Errors;

public static class CareerExplorationErrors
{
    public const int MaxCareersToCompare = 3;

    public static readonly Error CareerNotFound =
        Error.NotFound("CareerExploration.CareerNotFound", "The specified career was not found.");

    public static readonly Error FavoriteCareerNotFound =
        Error.NotFound("CareerExploration.FavoriteCareerNotFound", "The specified favorite career was not found.");

    public static readonly Error DuplicateCareerName =
        Error.Conflict("CareerExploration.DuplicateCareerName", "A career with the same name already exists.");

    public static readonly Error DuplicateFavoriteCareer =
        Error.Conflict("CareerExploration.DuplicateFavoriteCareer",
            "The career is already in the student's favorites.");

    public static readonly Error ComparisonLimitExceeded =
        Error.Validation("CareerExploration.ComparisonLimitExceeded",
            $"Only up to {MaxCareersToCompare} careers can be compared at the same time.");

    public static readonly Error InvalidStudentId =
        Error.Validation("CareerExploration.InvalidStudentId", "A valid studentId is required.");

    public static readonly Error CompatibilityNotAvailable =
        Error.NotFound("CareerExploration.CompatibilityNotAvailable",
            "The student does not have a vocational profile yet.");

    public static readonly Error DatabaseError =
        Error.Failure("CareerExploration.DatabaseError", "A database error occurred.");
}
