using System.Text;

namespace VocaFy.NextPath.Platform.Iam.Application;

internal static class PasswordPolicy
{
    public static bool IsValid(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        var byteCount = Encoding.UTF8.GetByteCount(password);
        return password.Length >= 12 && byteCount <= 72 &&
               password.Any(char.IsLetter) && password.Any(char.IsDigit);
    }
}
