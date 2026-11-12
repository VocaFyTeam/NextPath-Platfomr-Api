using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound("Iam.UserNotFound", "The specified user was not found.");

    public static readonly Error StudentNotFound =
        Error.NotFound("Iam.StudentNotFound", "The specified student was not found.");

    public static readonly Error PsychologistNotFound =
        Error.NotFound("Iam.PsychologistNotFound", "The specified psychologist was not found.");

    public static readonly Error EmailAlreadyTaken =
        Error.Conflict("Iam.EmailAlreadyTaken", "The email is already registered.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Iam.InvalidCredentials", "Invalid email or password.");

    public static readonly Error InvalidRole =
        Error.Validation("Iam.InvalidRole", "The role must be 'student' or 'psychologist'.");

    public static readonly Error WeakPassword =
        Error.Validation("Iam.WeakPassword", "Password must be 12 to 72 UTF-8 bytes and contain a letter and a number.");

    public static readonly Error MissingStudentData =
        Error.Validation("Iam.MissingStudentData", "Students must send schoolOrUniversity and currentCycle.");

    public static readonly Error MissingPsychologistData =
        Error.Validation("Iam.MissingPsychologistData", "Psychologists must send professionalLicense and specialty.");

    public static readonly Error InvalidLinkingCode =
        Error.Validation("Iam.InvalidLinkingCode", "No psychologist has the given linking code.");

    public static readonly Error InvalidOrExpiredResetToken =
        Error.Validation("Iam.InvalidOrExpiredResetToken", "The reset token is invalid, used or expired.");

    public static readonly Error ProfileOwnershipRequired =
        Error.Forbidden("Iam.ProfileOwnershipRequired", "You can only modify your own profile.");
}
