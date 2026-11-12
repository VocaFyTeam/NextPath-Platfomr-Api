namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model;

public enum CareerExplorationError
{
    None,
    CareerNotFound,
    FavoriteCareerNotFound,
    DuplicateCareerName,
    DuplicateFavoriteCareer,
    ComparisonLimitExceeded,
    InvalidStudentId,
    CompatibilityNotAvailable,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
