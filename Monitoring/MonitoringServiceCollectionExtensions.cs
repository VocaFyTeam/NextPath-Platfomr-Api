
using Microsoft.Extensions.DependencyInjection;

using VocaFy.NextPath.Platform.Monitoring.Application.Acl;
using VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;
using VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.Monitoring.Infrastructure.Reporting.AI.Services;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Monitoring;

public static class MonitoringServiceCollectionExtensions
{
    public static IServiceCollection AddMonitoringContext(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IGroupComparisonReportRepository, GroupComparisonReportRepository>();
        services.AddScoped<IHistoricalProfileRepository, HistoricalProfileRepository>();
        services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
        services.AddScoped<IVocationalReportRepository, VocationalReportRepository>();

        // Command services
        services.AddScoped<IGroupComparisonReportCommandService, GroupComparisonReportCommandService>();
        services.AddScoped<IHistoricalProfileCommandService, HistoricalProfileCommandService>();
        services.AddScoped<IStudentGroupCommandService, StudentGroupCommandService>();
        services.AddScoped<IVocationalReportCommandService, VocationalReportCommandService>();

        // Query services
        services.AddScoped<IGroupComparisonReportQueryService, GroupComparisonReportQueryService>();
        services.AddScoped<IHistoricalProfileQueryService, HistoricalProfileQueryService>();
        services.AddScoped<IStudentGroupQueryService, StudentGroupQueryService>();
        services.AddScoped<IVocationalReportQueryService, VocationalReportQueryService>();

        // External services
        services.AddScoped<IAIReportService, AIReportService>();
        services.AddScoped<IExternalIamService, ExternalIamService>();

        // Context facade
        services.AddScoped<IMonitoringContextFacade, MonitoringContextFacade>();

        return services;
    }
}
