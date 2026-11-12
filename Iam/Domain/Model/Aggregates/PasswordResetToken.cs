using System.Security.Cryptography;
using System.Text;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;

public class PasswordResetToken : AggregateRoot
{
    public static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(30);

    public PasswordResetToken()
    {
        Token = string.Empty;
    }

    public PasswordResetToken(UserId ownerUserId, string plainToken) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plainToken);
        Id = PasswordResetTokenId.New();
        UserId = ownerUserId;
        Token = HashToken(plainToken);
        ExpirationDate = DateTimeOffset.UtcNow.Add(Lifetime);
        IsUsed = false;
    }

    public PasswordResetTokenId Id { get; private set; }
    public UserId UserId { get; private set; }

    public string Token { get; private set; }

    public DateTimeOffset ExpirationDate { get; private set; }
    public bool IsUsed { get; private set; }

    public bool IsValid(DateTimeOffset now) => !IsUsed && now < ExpirationDate;

    public void MarkAsUsed() => IsUsed = true;

    public static string GeneratePlainToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public static string HashToken(string plainToken) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(plainToken.Trim().ToUpperInvariant())));
}
