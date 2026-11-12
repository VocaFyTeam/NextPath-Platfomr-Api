namespace VocaFy.NextPath.Platform.Iam.Domain.Model;

public enum IamError
{
    None,
    UserNotFound,
    StudentNotFound,
    PsychologistNotFound,
    EmailAlreadyTaken,
    InvalidCredentials,
    InvalidRole,
    WeakPassword,
    MissingStudentData,
    MissingPsychologistData,
    InvalidLinkingCode,
    InvalidOrExpiredResetToken,
    ProfileOwnershipRequired,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
