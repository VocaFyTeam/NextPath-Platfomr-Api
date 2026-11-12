using System.Text.Json.Serialization;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Events;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;

public class User : AggregateRoot
{
    public User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    public User(string emailAddress, string hashedPassword, EUserRole userRole) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);

        Id = UserId.New();
        Email = NormalizeEmail(emailAddress);
        PasswordHash = hashedPassword;
        Role = userRole;
        IsSessionActive = false;
        CreatedAt = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserRegisteredEvent(Id, Email, Role));
    }

    public UserId Id { get; private set; }
    public string Email { get; private set; }

    [JsonIgnore] public string PasswordHash { get; private set; }

    public EUserRole Role { get; private set; }
    public bool IsSessionActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public bool IsStudent() => Role == EUserRole.Student;
    public bool IsPsychologist() => Role == EUserRole.Psychologist;

    public void StartSession() => IsSessionActive = true;

    public void EndSession() => IsSessionActive = false;

    public void ChangePassword(string hashedPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);
        PasswordHash = hashedPassword;
        EndSession();
    }

    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
