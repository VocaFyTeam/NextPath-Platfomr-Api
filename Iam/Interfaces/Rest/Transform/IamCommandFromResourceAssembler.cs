using VocaFy.NextPath.Platform.Iam.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Iam.Interfaces.Rest.Transform;

public static class IamCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource r) =>
        new(r.Email, r.Password, r.Role, r.SchoolOrUniversity, r.CurrentCycle, r.LinkedPsychologistCode,
            r.ProfessionalLicense, r.Specialty);

    public static SignInCommand ToCommandFromResource(SignInResource r) => new(r.Email, r.Password);

    public static RequestPasswordResetCommand ToCommandFromResource(RequestPasswordResetResource r) => new(r.Email);

    public static ResetPasswordCommand ToCommandFromResource(ResetPasswordResource r) =>
        new(r.Token, r.NewPassword);

    public static UpdateStudentCommand ToCommandFromResource(Guid studentId, Guid requesterUserId,
        UpdateStudentResource r) =>
        new(studentId, requesterUserId, r.SchoolOrUniversity, r.CurrentCycle, r.LinkedPsychologistCode);

    public static UpdatePsychologistCommand ToCommandFromResource(Guid psychologistId, Guid requesterUserId,
        UpdatePsychologistResource r) =>
        new(psychologistId, requesterUserId, r.ProfessionalLicense, r.Specialty);
}
