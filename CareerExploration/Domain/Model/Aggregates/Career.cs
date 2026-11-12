using System.Globalization;
using System.Text;
using NextPath.Platform.CareerExploration.Domain.Model.Commands;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;
using NextPath.Platform.Shared.Domain.Model.Entities;

namespace NextPath.Platform.CareerExploration.Domain.Model.Aggregates;

/// <summary>
///     Career of the catalog (HU15, HU16). The catalog is read-only for the student;
///     it is loaded by the seeder (see <c>CareerCatalogSeeder</c>).
/// </summary>
public class Career : AggregateRoot, IAuditableEntity
{
    // Required by EF Core. Non-nullable members get safe defaults.
    protected Career()
    {
        Name = Emoji = Area = ImageUrl = ShortDescription = Description = string.Empty;
        DurationLabel = Modality = ModalityDetail = MainField = DemandLevel = string.Empty;
        Universities = [];
        FieldOfWork = [];
        Skills = [];
        Projection = [];
        Recommendations = [];
        AverageSalary = new SalaryRange(0, 0);
        SalaryRange = new SalaryRange(0, 0);
        Profile = new RiasecProfile(0, 0, 0, 0, 0);
    }

    public Career(CreateCareerCommand command)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Area);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Modality);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.DurationYears);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Semesters);
        ArgumentOutOfRangeException.ThrowIfNegative(command.EmployabilityRate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.EmployabilityRate, 100);
        ArgumentOutOfRangeException.ThrowIfNegative(command.BaseCompatibility);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.BaseCompatibility, 100);

        Name = command.Name.Trim();
        Emoji = command.Emoji;
        Area = command.Area.Trim();
        ImageUrl = command.ImageUrl;
        ShortDescription = command.ShortDescription;
        Description = command.Description;
        DurationYears = command.DurationYears;
        DurationLabel = command.DurationLabel;
        Semesters = command.Semesters;
        Modality = command.Modality.Trim();
        ModalityDetail = string.IsNullOrWhiteSpace(command.ModalityDetail) ? command.Modality : command.ModalityDetail;
        Universities = command.Universities;
        FieldOfWork = command.FieldOfWork;
        MainField = command.MainField;
        Skills = command.Skills;
        AverageSalary = command.AverageSalary;
        SalaryRange = command.SalaryRange;
        EmployabilityRate = command.EmployabilityRate;
        DemandLevel = command.DemandLevel;
        BaseCompatibility = command.BaseCompatibility;
        Profile = command.Profile;
        Projection = command.Projection;
        Recommendations = command.Recommendations;
    }

    public CareerId Id { get; private set; }
    public string Name { get; private set; }
    public string Emoji { get; private set; }
    public string Area { get; private set; }
    public string ImageUrl { get; private set; }
    public string ShortDescription { get; private set; }
    public string Description { get; private set; }
    public int DurationYears { get; private set; }
    public string DurationLabel { get; private set; }
    public int Semesters { get; private set; }
    public string Modality { get; private set; }
    public string ModalityDetail { get; private set; }
    public IReadOnlyList<University> Universities { get; private set; }
    public IReadOnlyList<string> FieldOfWork { get; private set; }
    public string MainField { get; private set; }
    public IReadOnlyList<string> Skills { get; private set; }
    public SalaryRange AverageSalary { get; private set; }
    public SalaryRange SalaryRange { get; private set; }
    public int EmployabilityRate { get; private set; }
    public string DemandLevel { get; private set; }

    /// <summary>Compatibility used when the student has no vocational profile yet.</summary>
    public int BaseCompatibility { get; private set; }

    public RiasecProfile Profile { get; private set; }
    public IReadOnlyList<ProjectionStage> Projection { get; private set; }
    public IReadOnlyList<string> Recommendations { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>True when the career is taught at the university with the given acronym (case-insensitive).</summary>
    public bool IsTaughtAt(string? acronym)
    {
        if (string.IsNullOrWhiteSpace(acronym)) return true;
        return Universities.Any(u => string.Equals(u.Acronym, acronym, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///     Free-text search, ignoring case and accents. Every word typed must appear in the
    ///     name, area, descriptions, main field or fields of work (same rule the frontend had).
    /// </summary>
    public bool MatchesText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return true;

        var haystack = Normalize(string.Join(' ', new[] { Name, Area, ShortDescription, MainField }
            .Concat(FieldOfWork)));

        return Normalize(text)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .All(word => haystack.Contains(word, StringComparison.Ordinal));
    }

    private static string Normalize(string value)
    {
        var decomposed = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        foreach (var character in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                builder.Append(character);
        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
