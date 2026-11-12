using System.Security.Cryptography;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;

public class Psychologist : AggregateRoot
{
    private const string LinkingCodeAlphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public Psychologist()
    {
        ProfessionalLicense = string.Empty;
        Specialty = string.Empty;
        UniqueLinkingCode = string.Empty;
    }

    public Psychologist(UserId ownerUserId, string license, string specialtyName, string linkingCode) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(linkingCode);
        Id = PsychologistId.New();
        UserId = ownerUserId;
        UniqueLinkingCode = NormalizeLinkingCode(linkingCode)!;
        UpdateProfile(license, specialtyName);
    }

    public PsychologistId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string ProfessionalLicense { get; private set; }
    public string Specialty { get; private set; }

    public string UniqueLinkingCode { get; private set; }

    public Psychologist UpdateProfile(string professionalLicense, string specialty)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(professionalLicense);
        ArgumentException.ThrowIfNullOrWhiteSpace(specialty);
        ProfessionalLicense = professionalLicense.Trim();
        Specialty = specialty.Trim();
        return this;
    }

    public static string GenerateLinkingCode() =>
        "PSY-" + RandomNumberGenerator.GetString(LinkingCodeAlphabet, 6);

    public static string? NormalizeLinkingCode(string? code) =>
        string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant();
}
