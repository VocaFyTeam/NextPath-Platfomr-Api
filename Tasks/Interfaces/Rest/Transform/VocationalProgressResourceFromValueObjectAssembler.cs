using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Tasks.Interfaces.Rest.Transform;

public static class VocationalProgressResourceFromValueObjectAssembler
{
    public static VocationalProgressResource ToResource(VocationalProgress progress)
    {
        return new VocationalProgressResource(
            progress.StudentId,
            progress.Percentage,
            progress.CompletedTests,
            progress.TotalTests,
            progress.CompletedTasks,
            progress.TotalTasks,
            progress.Favorites);
    }
}
