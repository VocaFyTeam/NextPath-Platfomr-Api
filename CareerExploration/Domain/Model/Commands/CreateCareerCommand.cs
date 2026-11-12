using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;

public record CreateCareerCommand(
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
    IReadOnlyList<University> Universities,
    IReadOnlyList<string> FieldOfWork,
    string MainField,
    IReadOnlyList<string> Skills,
    SalaryRange AverageSalary,
    SalaryRange SalaryRange,
    int EmployabilityRate,
    string DemandLevel,
    int BaseCompatibility,
    RiasecProfile Profile,
    IReadOnlyList<ProjectionStage> Projection,
    IReadOnlyList<string> Recommendations);
