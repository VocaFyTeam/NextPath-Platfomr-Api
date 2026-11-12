using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices;

public interface IAIReportService
{

    Task<string> GenerateTestReportAsync(HistoricalProfile profile, CancellationToken cancellationToken);
    
    Task<string> GenerateIndividualEvolutionReportAsync(HistoricalProfile profile,
        CancellationToken cancellationToken);
    
    Task<string> GeneratePsychologicalReportAsync(HistoricalProfile profile, CancellationToken cancellationToken);
    
    Task<string> GenerateAutomaticConclusionsAsync(IReadOnlyCollection<GroupMonitoringSummary> summaries,
        CancellationToken cancellationToken);
}
