using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

public static class TestQuestionResourceFromEntityAssembler
{
    public static TestQuestionResource ToResourceFromEntity(TestQuestion entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new TestQuestionResource(
            entity.Id.Value,
            entity.VocationalTestId.Value,
            entity.Order,
            AssessmentsEnumParser.ToText(entity.Type),
            entity.Text,
            entity.Area is null ? null : AssessmentsEnumParser.ToText(entity.Area.Value),
            entity.Placeholder,
            entity.Options.Select(option => new QuestionOptionResource(
                option.Code, option.Text, AssessmentsEnumParser.ToText(option.Area))));
    }
}
