namespace NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>Compatibility between a student's vocational profile and a career (HU17, TS10).</summary>
public record CareerCompatibility(CareerId CareerId, StudentId StudentId, int Percentage)
{
    public ECompatibilityLevel Level => Percentage.ToCompatibilityLevel();
}
