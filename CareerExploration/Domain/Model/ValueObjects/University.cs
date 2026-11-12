namespace NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>University that offers a career (stored as JSON inside the career row).</summary>
public record University(string Name, string Acronym, string StudyPlanUrl);
