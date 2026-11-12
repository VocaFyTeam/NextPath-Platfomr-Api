using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NextPath.Platform.CareerExploration.Application.CommandServices;
using NextPath.Platform.CareerExploration.Domain.Model;
using NextPath.Platform.CareerExploration.Domain.Model.Commands;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Infrastructure.Persistence.Seed;

/// <summary>
///     Loads the initial career catalog (the same 6 careers the frontend had in db.json).
///     It is idempotent: a career whose name already exists is skipped.
/// </summary>
public static class CareerCatalogSeeder
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static async Task SeedCareerCatalogAsync(this IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var commandService = scope.ServiceProvider.GetRequiredService<ICareerCommandService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("CareerCatalogSeeder");

        var relativePath = Path.Combine("CareerExploration", "Infrastructure", "Persistence", "Seed", "careers.json");
        // First next to the .dll (copied by the .csproj); then the project folder (dotnet run).
        var path = new[]
            {
                Path.Combine(AppContext.BaseDirectory, relativePath),
                Path.Combine(Directory.GetCurrentDirectory(), relativePath)
            }
            .FirstOrDefault(File.Exists);
        if (path is null)
        {
            logger.LogWarning("Career seed file {File} not found. The catalog will stay empty.", relativePath);
            return;
        }

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var seeds = JsonSerializer.Deserialize<List<CareerSeed>>(json, Options) ?? [];

        foreach (var seed in seeds)
        {
            var result = await commandService.Handle(seed.ToCommand(), cancellationToken);
            if (result.IsFailure && !Equals(result.Error, CareerExplorationError.DuplicateCareerName))
                logger.LogWarning("Could not seed career {Name}: {Message}", seed.Name, result.Message);
        }
    }

    // Mirrors careers.json. The "id" field of the frontend db.json is not used: the database assigns it.
    private record CareerSeed(
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
        List<University> Universities,
        List<string> FieldOfWork,
        string MainField,
        List<string> Skills,
        SalaryRange AverageSalary,
        SalaryRange SalaryRange,
        int EmployabilityRate,
        string DemandLevel,
        int BaseCompatibility,
        RiasecProfile Profile,
        List<ProjectionStage> Projection,
        List<string> Recommendations)
    {
        public CreateCareerCommand ToCommand() => new(
            Name, Emoji, Area, ImageUrl, ShortDescription, Description, DurationYears, DurationLabel, Semesters,
            Modality, ModalityDetail, Universities, FieldOfWork, MainField, Skills, AverageSalary, SalaryRange,
            EmployabilityRate, DemandLevel, BaseCompatibility, Profile, Projection, Recommendations);
    }
}
