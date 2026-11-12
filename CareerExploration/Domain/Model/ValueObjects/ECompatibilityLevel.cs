namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

public enum ECompatibilityLevel
{
    Low = 0,
    Medium = 1,
    High = 2,
    Excellent = 3
}

public static class CompatibilityLevelExtensions
{
    /// <summary>Same thresholds the frontend used (<c>Career.compatibilityLevel</c>).</summary>
    public static ECompatibilityLevel ToCompatibilityLevel(this int percentage) => percentage switch
    {
        >= 90 => ECompatibilityLevel.Excellent,
        >= 80 => ECompatibilityLevel.High,
        >= 65 => ECompatibilityLevel.Medium,
        _ => ECompatibilityLevel.Low
    };
}
