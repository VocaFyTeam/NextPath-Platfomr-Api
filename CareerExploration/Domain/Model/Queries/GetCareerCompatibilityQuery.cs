using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Domain.Model.Queries;

public record GetCareerCompatibilityQuery(CareerId CareerId, StudentId StudentId);
