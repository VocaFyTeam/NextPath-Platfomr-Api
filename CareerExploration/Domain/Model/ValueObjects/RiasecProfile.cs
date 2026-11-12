namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>
///     RIASEC (Holland) profile that characterizes a career, each dimension from 0 to 100.
///     The Assessments context compares it against the student's own scores.
/// </summary>
public record RiasecProfile(int Artistic, int Social, int Investigative, int Enterprising, int Conventional);
