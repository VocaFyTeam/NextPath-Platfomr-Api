using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Services;

public static class InterventionRecommendationPolicy
{
  
    public static IReadOnlyList<InterventionRecommendation> Recommend(IEnumerable<EStudentFlag> flags)
    {
        var recommendations = new List<InterventionRecommendation>();

        void Add(EInterventionRecommendation type, EStudentFlag reason)
        {
            if (recommendations.All(recommendation => recommendation.Type != type))
                recommendations.Add(new InterventionRecommendation(type, reason));
        }

        foreach (var flag in flags.Distinct())
            switch (flag)
            {
                case EStudentFlag.Inactivity:
                    Add(EInterventionRecommendation.ScheduleOneOnOneSession, flag);
                    Add(EInterventionRecommendation.SendFollowUpReminder, flag);
                    break;
                case EStudentFlag.LowParticipation:
                    Add(EInterventionRecommendation.AssignExplorationWorkshop, flag);
                    break;
                case EStudentFlag.Inconsistent:
                    Add(EInterventionRecommendation.ReviewAnswerConsistency, flag);
                    Add(EInterventionRecommendation.ScheduleOneOnOneSession, flag);
                    break;
            }

        return recommendations;
    }
}
