using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

public static class TestResultResourceFromEntityAssembler
{
    public static TestResultResource ToResourceFromEntity(TestResult entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var date = entity.CompletedAt ?? entity.CreatedAt?.UtcDateTime ?? DateTime.UtcNow;
        return new TestResultResource(
            entity.Id.Value,
            entity.StudentId,
            entity.VocationalTestId.Value,
            date.ToString("yyyy-MM-dd"),
            entity.Version,
            AssessmentsEnumParser.ToText(entity.Status),
            entity.Summary,
            new ScoresResource(entity.Scores.Artistic, entity.Scores.Social, entity.Scores.Investigative,
                entity.Scores.Enterprising, entity.Scores.Conventional),
            entity.CareerMatches.Select(match => new CareerMatchResource(match.CareerId, match.Compatibility)),
            entity.Answers.ToDictionary(answer => answer.QuestionId.ToString(), answer => answer.Value));
    }
}
