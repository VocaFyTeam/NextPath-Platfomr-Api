using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;

public record AddFavoriteCareerCommand(StudentId StudentId, CareerId CareerId);
