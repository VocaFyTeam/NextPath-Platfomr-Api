using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Domain.Model.Commands;

public record AddFavoriteCareerCommand(StudentId StudentId, CareerId CareerId);
