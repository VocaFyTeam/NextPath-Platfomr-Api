namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

/// <summary>
///     Value object with the 0-100 score of each affinity dimension (the student's vocational profile).
/// </summary>
public record AffinityProfile(int Artistic, int Social, int Investigative, int Enterprising, int Conventional)
{
    public AffinityProfile() : this(0, 0, 0, 0, 0)
    {
    }

    /// <summary>Holland code made of the three dominant dimensions, e.g. "ASI".</summary>
    public string HollandCode => string.Concat(DominantDimensions(3).Select(d => d.ToString()[0]));

    public int ScoreOf(EAffinityDimension dimension) => dimension switch
    {
        EAffinityDimension.Artistic => Artistic,
        EAffinityDimension.Social => Social,
        EAffinityDimension.Investigative => Investigative,
        EAffinityDimension.Enterprising => Enterprising,
        EAffinityDimension.Conventional => Conventional,
        _ => 0
    };

    public IEnumerable<EAffinityDimension> DominantDimensions(int count = 2) =>
        Enum.GetValues<EAffinityDimension>().OrderByDescending(ScoreOf).Take(count);
}
