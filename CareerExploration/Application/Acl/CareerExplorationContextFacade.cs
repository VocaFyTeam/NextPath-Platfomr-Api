using NextPath.Platform.CareerExploration.Application.QueryServices;
using NextPath.Platform.CareerExploration.Domain.Model.Queries;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.CareerExploration.Interfaces.Acl;

namespace NextPath.Platform.CareerExploration.Application.Acl;

public class CareerExplorationContextFacade(
    ICareerQueryService careerQueryService,
    IFavoriteCareerQueryService favoriteCareerQueryService)
    : ICareerExplorationContextFacade
{
    public async Task<int> CountFavoritesByStudentId(int studentId, CancellationToken cancellationToken = default)
    {
        var favorites = await favoriteCareerQueryService.Handle(
            new GetFavoriteCareersByStudentIdQuery(new StudentId(studentId)), cancellationToken);
        return favorites.Count();
    }

    public async Task<IReadOnlyList<FavoriteCareerSummary>> FetchFavoriteCareersByStudentId(int studentId,
        CancellationToken cancellationToken = default)
    {
        var favorites = (await favoriteCareerQueryService.Handle(
            new GetFavoriteCareersByStudentIdQuery(new StudentId(studentId)), cancellationToken)).ToList();
        if (favorites.Count == 0) return [];

        var careers = await careerQueryService.Handle(
            new GetCareersByIdsQuery(favorites.Select(favorite => favorite.CareerId).ToList()), cancellationToken);

        return careers.Select(career => new FavoriteCareerSummary(career.Id, career.Name, career.Area)).ToList();
    }

    public async Task<IReadOnlyList<CareerProfileSummary>> FetchCareerProfiles(
        CancellationToken cancellationToken = default)
    {
        var careers = await careerQueryService.Handle(new GetAllCareersQuery(), cancellationToken);
        return careers
            .Select(career => new CareerProfileSummary(
                career.Id, career.Name,
                career.Profile.Artistic, career.Profile.Social, career.Profile.Investigative,
                career.Profile.Enterprising, career.Profile.Conventional))
            .ToList();
    }
}
