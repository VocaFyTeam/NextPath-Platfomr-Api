using System.Security.Cryptography;
using System.Text;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Entities;

public partial class ProfessionalObservation
{
    public const int MaxObservationLength = 2000;
    public const int MaxAgreementsLength = 1000;
    
    public ProfessionalObservation()
    {
        ObservationText = string.Empty;
        Agreements = string.Empty;
        DigitalSignature = string.Empty;
    }
    
    public ProfessionalObservation(HistoricalProfileId historicalProfileId, StudentId studentId,
        PsychologistId psychologistId, string observationText, string? agreements, DateTimeOffset? sessionDate) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(observationText);
        HistoricalProfileId = historicalProfileId;
        StudentId = studentId;
        PsychologistId = psychologistId;
        ObservationText = observationText.Trim();
        Agreements = agreements?.Trim() ?? string.Empty;
        SessionDate = sessionDate;
        DigitalSignature = ComputeDigitalSignature();
    }
    
    public ProfessionalObservationId Id { get; }
    
    public HistoricalProfileId HistoricalProfileId { get; private set; }
    
    public StudentId StudentId { get; private set; }
    
    public PsychologistId PsychologistId { get; private set; }
    
    public string ObservationText { get; private set; }
    
    public string Agreements { get; private set; }
    
    public DateTimeOffset? SessionDate { get; private set; }
    
    public string DigitalSignature { get; private set; }

    private string ComputeDigitalSignature()
    {
        var content = string.Join("|", PsychologistId.Value, StudentId.Value, ObservationText, Agreements,
            SessionDate?.ToString("O") ?? string.Empty);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));
    }
}
