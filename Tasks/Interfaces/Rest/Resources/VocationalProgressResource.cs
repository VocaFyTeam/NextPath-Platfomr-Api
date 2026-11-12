namespace NextPath.Platform.Tasks.Interfaces.Rest.Resources;

public record VocationalProgressResource(
    int StudentId,
    int Percentage,
    int CompletedTests,
    int TotalTests,
    int CompletedTasks,
    int TotalTasks,
    int Favorites);
