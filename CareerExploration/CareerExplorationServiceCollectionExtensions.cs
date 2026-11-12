using Microsoft.Extensions.DependencyInjection;
using NextPath.Platform.CareerExploration.Application.Acl;
using NextPath.Platform.CareerExploration.Application.CommandServices;
using NextPath.Platform.CareerExploration.Application.Internal.CommandServices;
using NextPath.Platform.CareerExploration.Application.Internal.OutboundServices;
using NextPath.Platform.CareerExploration.Application.Internal.QueryServices;
using NextPath.Platform.CareerExploration.Application.QueryServices;
using NextPath.Platform.CareerExploration.Domain.Repositories;
using NextPath.Platform.CareerExploration.Infrastructure.OutboundServices;
using NextPath.Platform.CareerExploration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NextPath.Platform.CareerExploration.Interfaces.Acl;

namespace NextPath.Platform.CareerExploration;

public static class CareerExplorationServiceCollectionExtensions
{
    /// <summary>The ONLY line this context adds to Program.cs: <c>builder.Services.AddCareerExplorationContext();</c></summary>
    public static IServiceCollection AddCareerExplorationContext(this IServiceCollection services)
    {
        services.AddScoped<ICareerRepository, CareerRepository>();
        services.AddScoped<IFavoriteCareerRepository, FavoriteCareerRepository>();

        services.AddScoped<ICareerCommandService, CareerCommandService>();
        services.AddScoped<IFavoriteCareerCommandService, FavoriteCareerCommandService>();
        services.AddScoped<ICareerQueryService, CareerQueryService>();
        services.AddScoped<IFavoriteCareerQueryService, FavoriteCareerQueryService>();

        // TEMPORARY: replace with the real adapter over Assessments' facade when it exists.
        services.AddScoped<IAssessmentCompatibilityProvider, StubAssessmentCompatibilityProvider>();

        services.AddScoped<ICareerExplorationContextFacade, CareerExplorationContextFacade>();
        return services;
    }
}
