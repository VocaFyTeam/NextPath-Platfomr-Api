using System.ComponentModel.DataAnnotations;

namespace VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

public record CreateFavoriteCareerResource(
    [property: Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive number")]
    int StudentId,
    [property: Range(1, int.MaxValue, ErrorMessage = "CareerId must be a positive number")]
    int CareerId);
