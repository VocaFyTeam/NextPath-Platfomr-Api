using System.Text;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Infrastructure.Reporting.AI.Services;

public class AIReportService : IAIReportService
{
    public Task<string> GenerateTestReportAsync(HistoricalProfile profile, CancellationToken cancellationToken)
    {
        var text = new StringBuilder();
        text.AppendLine($"Reporte de pruebas del estudiante {profile.StudentId.Value}.");
        text.AppendLine(Header(profile));
        text.AppendLine($"Alertas de participación: {DescribeFlags(profile)}.");
        return Task.FromResult(text.ToString().Trim());
    }
    
    public Task<string> GenerateIndividualEvolutionReportAsync(HistoricalProfile profile,
        CancellationToken cancellationToken)
    {
        var text = new StringBuilder();
        text.AppendLine($"Reporte de evolución vocacional del estudiante {profile.StudentId.Value}.");
        text.AppendLine(Header(profile));
        text.AppendLine($"Alertas activas: {DescribeFlags(profile)}.");
        text.AppendLine($"Observaciones profesionales registradas: {profile.Observations.Count}.");
        var latest = profile.Observations.OrderByDescending(observation => observation.CreatedAt).FirstOrDefault();
        if (latest is not null) text.AppendLine($"Última observación: {latest.ObservationText}");
        return Task.FromResult(text.ToString().Trim());
    }
    
    public Task<string> GeneratePsychologicalReportAsync(HistoricalProfile profile,
        CancellationToken cancellationToken)
    {
        var text = new StringBuilder();
        text.AppendLine($"Informe psicológico del estudiante {profile.StudentId.Value}.");
        text.AppendLine(Header(profile));
        text.AppendLine($"Alertas activas: {DescribeFlags(profile)}.");
        foreach (var observation in profile.Observations.OrderBy(observation => observation.CreatedAt))
        {
            text.AppendLine($"- {observation.SessionDate?.ToString("yyyy-MM-dd") ?? "Sin fecha de sesión"}: {observation.ObservationText}");
            if (observation.Agreements.Length > 0) text.AppendLine($"  Acuerdos: {observation.Agreements}");
        }

        var recommendations = profile.GetInterventionRecommendations();
        if (recommendations.Count > 0)
            text.AppendLine("Intervenciones sugeridas: " +
                            string.Join(", ", recommendations.Select(recommendation => Describe(recommendation.Type))) +
                            ".");
        return Task.FromResult(text.ToString().Trim());
    }
    
    public Task<string> GenerateAutomaticConclusionsAsync(IReadOnlyCollection<GroupMonitoringSummary> summaries,
        CancellationToken cancellationToken)
    {
        var text = new StringBuilder();
        text.AppendLine($"Comparación de {summaries.Count} grupos.");
        foreach (var summary in summaries)
            text.AppendLine(
                $"{summary.GroupName}: {summary.StudentCount} estudiantes, {summary.AtRiskCount} con alertas ({Percentage(summary.AtRiskCount, summary.StudentCount)}%), " +
                $"{summary.InactiveCount} inactivos, {summary.LowParticipationCount} con baja participación, " +
                $"{summary.InconsistentCount} con respuestas inconsistentes.");

        var withStudents = summaries.Where(summary => summary.StudentCount > 0).ToList();
        if (withStudents.Count > 0)
        {
            var highest = withStudents.MaxBy(summary => Percentage(summary.AtRiskCount, summary.StudentCount))!;
            text.AppendLine($"{highest.GroupName} concentra el mayor porcentaje de estudiantes con alertas y requiere priorizar el acompañamiento.");
        }
        else
        {
            text.AppendLine("Los grupos seleccionados aún no tienen estudiantes asignados.");
        }

        return Task.FromResult(text.ToString().Trim());
    }

    private static string Header(HistoricalProfile profile)
    {
        var now = DateTimeOffset.UtcNow;
        return $"Psicólogo asignado: {profile.AssignedPsychologistId.Value}. Última actividad: {profile.LastActivityAt:yyyy-MM-dd} " +
               $"({profile.DaysInactive(now)} días sin actividad).";
    }

    private static string DescribeFlags(HistoricalProfile profile)
    {
        return profile.Flags.Count == 0
            ? "ninguna"
            : string.Join(", ", profile.Flags.Select(flag => flag switch
            {
                EStudentFlag.Inactivity => "inactividad prolongada",
                EStudentFlag.Inconsistent => "respuestas inconsistentes",
                EStudentFlag.LowParticipation => "baja participación",
                _ => flag.ToString()
            }));
    }

    private static string Describe(EInterventionRecommendation type)
    {
        return type switch
        {
            EInterventionRecommendation.ScheduleOneOnOneSession => "convocar a sesión 1 a 1",
            EInterventionRecommendation.AssignExplorationWorkshop => "asignar taller de exploración",
            EInterventionRecommendation.SendFollowUpReminder => "enviar recordatorio de seguimiento",
            EInterventionRecommendation.ReviewAnswerConsistency => "revisar la consistencia de las respuestas",
            _ => type.ToString()
        };
    }

    private static int Percentage(int part, int total)
    {
        return total == 0 ? 0 : (int)Math.Round(part * 100.0 / total);
    }
}
