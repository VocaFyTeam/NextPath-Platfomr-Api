using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

public static class SubmitTestResultCommandFromResourceAssembler
{
    public static SubmitTestResultCommand ToCommandFromResource(SubmitTestResultResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new SubmitTestResultCommand(
            resource.StudentId,
            resource.TestId,
            AssessmentsEnumParser.ToStatus(resource.Status),
            AssessmentsEnumParser.ToAnswers(resource.Answers),
            ToCareerMatches(resource.CareerMatches));
    }

    public static UpdateTestResultCommand ToUpdateCommandFromResource(Guid testResultId,
        SubmitTestResultResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new UpdateTestResultCommand(
            testResultId,
            AssessmentsEnumParser.ToStatus(resource.Status),
            AssessmentsEnumParser.ToAnswers(resource.Answers),
            ToCareerMatches(resource.CareerMatches));
    }

    private static List<CareerMatch> ToCareerMatches(IEnumerable<CareerMatchResource>? matches) =>
        (matches ?? Enumerable.Empty<CareerMatchResource>()).Select(match => new CareerMatch(match.CareerId, match.Compatibility)).ToList();
}
