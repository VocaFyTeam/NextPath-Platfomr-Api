namespace NextPath.Platform.CareerExploration.Interfaces.Acl;

/// <summary>Favorite career of a student, with the data other contexts need (e.g. Monitoring).</summary>
public record FavoriteCareerSummary(int CareerId, string Name, string Area);

/// <summary>RIASEC profile of a career. Assessments uses it to calculate career matches.</summary>
public record CareerProfileSummary(
    int CareerId,
    string Name,
    int Artistic,
    int Social,
    int Investigative,
    int Enterprising,
    int Conventional);

/// <summary>
///     Public contract of the CareerExploration context for the other bounded contexts.
///     Only primitives and records cross this boundary, never the aggregates.
/// </summary>
public interface ICareerExplorationContextFacade
{
    Task<int> CountFavoritesByStudentId(int studentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FavoriteCareerSummary>> FetchFavoriteCareersByStudentId(int studentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CareerProfileSummary>> FetchCareerProfiles(CancellationToken cancellationToken = default);
}
