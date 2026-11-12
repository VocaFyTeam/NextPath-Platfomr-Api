namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

/// <summary>
///     Kind of session. It is also the discriminator column sessions.session_type (INDIVIDUAL / GROUP).
/// </summary>
public enum ESessionType
{
    Individual,
    Group
}
