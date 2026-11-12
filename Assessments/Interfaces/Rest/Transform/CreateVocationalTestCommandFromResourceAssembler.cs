using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

public static class CreateVocationalTestCommandFromResourceAssembler
{
    public static CreateVocationalTestCommand ToCommandFromResource(CreateVocationalTestResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        var questions = (resource.Questions ?? Enumerable.Empty<CreateTestQuestionResource>()).Select(question => new TestQuestionData(
            question.Order,
            AssessmentsEnumParser.ToTestType(question.Type),
            question.Text,
            AssessmentsEnumParser.ToDimension(question.Area),
            question.Placeholder,
            (question.Options ?? Enumerable.Empty<QuestionOptionResource>()).Select(option => new QuestionOptionData(
                option.Id,
                option.Text,
                AssessmentsEnumParser.ToDimension(option.Area)
                ?? throw new ArgumentException($"Invalid area '{option.Area}' in option '{option.Id}'.")))))
            .ToList();

        return new CreateVocationalTestCommand(
            resource.Title,
            resource.ShortName ?? string.Empty,
            resource.Headline ?? string.Empty,
            resource.Description,
            resource.Instructions,
            AssessmentsEnumParser.ToTestType(resource.Type),
            resource.Featured,
            resource.Order,
            resource.Version ?? "1.0",
            resource.DurationMinutes,
            questions);
    }
}
