namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

/// <summary>
///     Compatibility (0-100) between the student's profile and one career of the catalog.
/// </summary>
public class CareerMatch
{
    public CareerMatch()
    {
    }

    public CareerMatch(Guid careerId, int compatibility)
    {
        CareerId = careerId;
        Compatibility = Math.Clamp(compatibility, 0, 100);
    }

    public Guid CareerId { get; private set; }
    public int Compatibility { get; private set; }
}
