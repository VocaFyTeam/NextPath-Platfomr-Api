namespace VocaFy.NextPath.Platform.Assessments.Domain.Model;

public enum AssessmentsError
{
    None,
    VocationalTestNotFound,
    TestResultNotFound,
    DuplicateVocationalTestTitle,
    InvalidAnswers,
    TestResultAlreadyCompleted,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
