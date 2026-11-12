using VocaFy.NextPath.Platform.Assessments.Application.CommandServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;
using VocaFy.NextPath.Platform.Assessments.Domain.Services;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Application.Internal.CommandServices;

public class TestResultCommandService(
    ITestResultRepository testResultRepository,
    IVocationalTestRepository vocationalTestRepository,
    IUnitOfWork unitOfWork) : ITestResultCommandService
{
    public async Task<Result<TestResult>> Handle(SubmitTestResultCommand command,
        CancellationToken cancellationToken)
    {
        var vocationalTest =
            await vocationalTestRepository.FindWithQuestionsByIdAsync(command.VocationalTestId, cancellationToken);
        if (vocationalTest is null)
            return Result<TestResult>.Failure(AssessmentsErrors.VocationalTestNotFound,
                AssessmentsError.VocationalTestNotFound);

        if (!vocationalTest.ContainsAllQuestions(command.Answers.Keys))
            return Result<TestResult>.Failure(AssessmentsErrors.InvalidAnswers, AssessmentsError.InvalidAnswers);

        var testResult = new TestResult(command.StudentId, vocationalTest);
        ApplyAnswers(testResult, vocationalTest, command.Status, command.Answers, command.CareerMatches);

        await testResultRepository.AddAsync(testResult, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<TestResult>.Success(testResult);
    }

    public async Task<Result<TestResult>> Handle(UpdateTestResultCommand command,
        CancellationToken cancellationToken)
    {
        var testResult = await testResultRepository.FindByResultIdAsync(command.TestResultId, cancellationToken);
        if (testResult is null)
            return Result<TestResult>.Failure(AssessmentsErrors.TestResultNotFound,
                AssessmentsError.TestResultNotFound);

        if (testResult.IsCompleted)
            return Result<TestResult>.Failure(AssessmentsErrors.TestResultAlreadyCompleted,
                AssessmentsError.TestResultAlreadyCompleted);

        var vocationalTest = await vocationalTestRepository.FindWithQuestionsByIdAsync(
            testResult.VocationalTestId.Value, cancellationToken);
        if (vocationalTest is null)
            return Result<TestResult>.Failure(AssessmentsErrors.VocationalTestNotFound,
                AssessmentsError.VocationalTestNotFound);

        if (!vocationalTest.ContainsAllQuestions(command.Answers.Keys))
            return Result<TestResult>.Failure(AssessmentsErrors.InvalidAnswers, AssessmentsError.InvalidAnswers);

        ApplyAnswers(testResult, vocationalTest, command.Status, command.Answers, command.CareerMatches);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<TestResult>.Success(testResult);
    }

    private static void ApplyAnswers(TestResult testResult, VocationalTest vocationalTest,
        ETestResultStatus status, IReadOnlyDictionary<Guid, string> answers, IEnumerable<CareerMatch> careerMatches)
    {
        if (status == ETestResultStatus.Completed)
        {
            var scores = AffinityAnalyzer.Analyze(vocationalTest.Questions, answers);
            testResult.Complete(answers, scores, careerMatches);
        }
        else
        {
            testResult.SaveProgress(answers);
        }
    }
}
