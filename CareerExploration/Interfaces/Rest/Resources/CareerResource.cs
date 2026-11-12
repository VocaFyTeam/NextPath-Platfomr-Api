namespace NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

public record UniversityResource(string Name, string Acronym, string StudyPlanUrl);

public record SalaryRangeResource(int Min, int Max);

public record RiasecProfileResource(int Artistic, int Social, int Investigative, int Enterprising, int Conventional);

public record ProjectionStageResource(string Period, string Role, string Salary);

/// <summary>Same JSON shape the frontend already consumes from db.json (<c>Career</c> entity).</summary>
public record CareerResource(
    int Id,
    string Name,
    string Emoji,
    string Area,
    string ImageUrl,
    string ShortDescription,
    string Description,
    int DurationYears,
    string DurationLabel,
    int Semesters,
    string Modality,
    string ModalityDetail,
    IEnumerable<UniversityResource> Universities,
    IEnumerable<string> FieldOfWork,
    string MainField,
    IEnumerable<string> Skills,
    SalaryRangeResource AverageSalary,
    SalaryRangeResource SalaryRange,
    int EmployabilityRate,
    string DemandLevel,
    int BaseCompatibility,
    RiasecProfileResource Profile,
    IEnumerable<ProjectionStageResource> Projection,
    IEnumerable<string> Recommendations);
