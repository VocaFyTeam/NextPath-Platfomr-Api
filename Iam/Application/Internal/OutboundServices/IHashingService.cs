namespace VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;

/**
 *     Hashing service interface
 *     This interface is used to hash and verify passwords
 */
public interface IHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}