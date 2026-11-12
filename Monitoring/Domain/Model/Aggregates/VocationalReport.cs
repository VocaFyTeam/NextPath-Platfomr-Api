using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;

public partial class VocationalReport : AggregateRoot
{
    public const int MaxContentSummaryLength = 4000;
    
    public VocationalReport()
    {
        ContentSummary = string.Empty;
    }
    
    public VocationalReport(StudentId studentId, PsychologistId psychologistId, EReportType reportType,
        string contentSummary) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentSummary);
        StudentId = studentId;
        PsychologistId = psychologistId;
        ReportType = reportType;
        ContentSummary = contentSummary.Trim();
        IsValidatedByPsychologist = false;
        AddDomainEvent(new VocationalReportGeneratedEvent(studentId, psychologistId, reportType));
    }
    
    public VocationalReportId Id { get; }
    
    public StudentId StudentId { get; private set; }
    
    public PsychologistId PsychologistId { get; private set; }
    
    public EReportType ReportType { get; private set; }
    
    public string ContentSummary { get; private set; }
    
    public bool IsValidatedByPsychologist { get; private set; }
    
    public DateTimeOffset? ValidatedAt { get; private set; }
    
    public bool IsOwnedBy(PsychologistId psychologistId)
    {
        return PsychologistId == psychologistId;
    }
    
    public void UpdateContentSummary(string contentSummary)
    {
        if (IsValidatedByPsychologist)
            throw new InvalidOperationException("A validated vocational report cannot be modified.");
        ArgumentException.ThrowIfNullOrWhiteSpace(contentSummary);
        ContentSummary = contentSummary.Trim();
    }
    
    public void ValidateReport()
    {
        if (IsValidatedByPsychologist) return;
        IsValidatedByPsychologist = true;
        ValidatedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new VocationalReportValidatedEvent(StudentId, PsychologistId));
    }
}
