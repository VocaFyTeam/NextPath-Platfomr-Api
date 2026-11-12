namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

/// <summary>
///     Overall progress of a student on the platform (HU21). Rule migrated from the frontend
///     (<c>VocationalProgress</c>): 50 % tests, 30 % tasks, 20 % favorite careers (goal: 3).
/// </summary>
public sealed record VocationalProgress
{
    public const double TestsWeight = 0.5;
    public const double TasksWeight = 0.3;
    public const double FavoritesWeight = 0.2;
    public const int FavoritesGoal = 3;

    public VocationalProgress(StudentId studentId, int completedTests, int totalTests, int completedTasks,
        int totalTasks, int favorites)
    {
        StudentId = studentId;
        CompletedTests = Math.Max(completedTests, 0);
        TotalTests = Math.Max(totalTests, 0);
        CompletedTasks = Math.Max(completedTasks, 0);
        TotalTasks = Math.Max(totalTasks, 0);
        Favorites = Math.Max(favorites, 0);

        var value = Ratio(CompletedTests, TotalTests) * TestsWeight
                    + Ratio(CompletedTasks, TotalTasks) * TasksWeight
                    + Ratio(Favorites, FavoritesGoal) * FavoritesWeight;

        Percentage = (int)Math.Round(value * 100, MidpointRounding.AwayFromZero);
    }

    public StudentId StudentId { get; }
    public int CompletedTests { get; }
    public int TotalTests { get; }
    public int CompletedTasks { get; }
    public int TotalTasks { get; }
    public int Favorites { get; }
    public int Percentage { get; }

    private static double Ratio(int done, int total) => total > 0 ? Math.Min((double)done / total, 1d) : 0d;
}
