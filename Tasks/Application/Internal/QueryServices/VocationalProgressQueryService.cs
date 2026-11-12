using VocaFy.NextPath.Platform.CareerExploration.Interfaces.Acl;
using VocaFy.NextPath.Platform.Tasks.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Tasks.Application.QueryServices;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Tasks.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Tasks.Application.Internal.QueryServices;

/// <summary>
///     Combines data of three contexts: its own tasks, the tests (Assessments, through a port) and the
///     favorite careers (CareerExploration, through its ACL facade).
/// </summary>
public class VocationalProgressQueryService(
    IStudentTaskRepository studentTaskRepository,
    IAssessmentProgressProvider assessmentProgressProvider,
    ICareerExplorationContextFacade careerExplorationContextFacade)
    : IVocationalProgressQueryService
{
    public async Task<VocationalProgress> Handle(GetVocationalProgressByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        var tasks = (await studentTaskRepository.FindByStudentIdAsync(query.StudentId, cancellationToken)).ToList();
        var assessments = await assessmentProgressProvider.GetAssessmentProgressAsync(query.StudentId,
            cancellationToken);
        var favorites = await careerExplorationContextFacade.CountFavoritesByStudentId(query.StudentId,
            cancellationToken);

        return new VocationalProgress(
            query.StudentId,
            assessments.CompletedTests,
            assessments.TotalTests,
            tasks.Count(task => task.IsCompleted),
            tasks.Count,
            favorites);
    }
}
