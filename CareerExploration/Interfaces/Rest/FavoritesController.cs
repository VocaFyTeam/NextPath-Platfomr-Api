using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using VocaFy.NextPath.Platform.CareerExploration.Application.CommandServices;
using VocaFy.NextPath.Platform.CareerExploration.Application.QueryServices;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Queries;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;
using VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest;

/// <summary>Favorite careers of a student (HU19, TS11).</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Favorites")]
// TODO(Iam): add [Authorize] and read the student id from the token instead of the query/body.
public class FavoritesController(
    IFavoriteCareerCommandService favoriteCareerCommandService,
    IFavoriteCareerQueryService favoriteCareerQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FavoriteCareerResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFavoritesByStudentId([FromQuery] int studentId,
        CancellationToken cancellationToken)
    {
        if (studentId <= 0)
            return CareerExplorationActionResultAssembler.ToBadRequest(
                this, problemDetailsFactory, CareerExplorationError.InvalidStudentId);

        var favorites = await favoriteCareerQueryService.Handle(
            new GetFavoriteCareersByStudentIdQuery(new StudentId(studentId)), cancellationToken);

        return Ok(favorites.Select(FavoriteCareerResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{favoriteId:int}")]
    [ProducesResponseType(typeof(FavoriteCareerResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFavoriteById(int favoriteId, CancellationToken cancellationToken)
    {
        var favorite = await favoriteCareerQueryService.Handle(
            new GetFavoriteCareerByIdQuery(new FavoriteCareerId(favoriteId)), cancellationToken);
        if (favorite is null)
            return CareerExplorationActionResultAssembler.ToNotFound(
                this, problemDetailsFactory, CareerExplorationError.FavoriteCareerNotFound);

        return Ok(FavoriteCareerResourceFromEntityAssembler.ToResourceFromEntity(favorite));
    }

    [HttpPost]
    [ProducesResponseType(typeof(FavoriteCareerResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddFavorite([FromBody] CreateFavoriteCareerResource resource,
        CancellationToken cancellationToken)
    {
        var command = AddFavoriteCareerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await favoriteCareerCommandService.Handle(command, cancellationToken);

        return CareerExplorationActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            favorite => CreatedAtAction(nameof(GetFavoriteById), new { favoriteId = (int)favorite.Id },
                FavoriteCareerResourceFromEntityAssembler.ToResourceFromEntity(favorite)));
    }

    [HttpDelete("{favoriteId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFavorite(int favoriteId, CancellationToken cancellationToken)
    {
        var result = await favoriteCareerCommandService.Handle(
            new RemoveFavoriteCareerCommand(new FavoriteCareerId(favoriteId)), cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : CareerExplorationActionResultAssembler.ToProblem(this, result, problemDetailsFactory);
    }
}
