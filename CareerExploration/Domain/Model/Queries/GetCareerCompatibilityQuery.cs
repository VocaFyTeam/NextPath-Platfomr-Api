using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Queries;

public record GetCareerCompatibilityQuery(CareerId CareerId, StudentId StudentId);
