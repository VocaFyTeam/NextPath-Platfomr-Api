using Microsoft.Extensions.DependencyInjection;
using VocaFy.NextPath.Platform.Tasks.Application.Acl;
using VocaFy.NextPath.Platform.Tasks.Application.CommandServices;
using VocaFy.NextPath.Platform.Tasks.Application.Internal.CommandServices;
using VocaFy.NextPath.Platform.Tasks.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Tasks.Application.Internal.QueryServices;
using VocaFy.NextPath.Platform.Tasks.Application.QueryServices;
using VocaFy.NextPath.Platform.Tasks.Domain.Repositories;
using VocaFy.NextPath.Platform.Tasks.Infrastructure.OutboundServices;
using VocaFy.NextPath.Platform.Tasks.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.Tasks.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Tasks;

public static class TasksServiceCollectionExtensions
{
    /// <summary>The ONLY line this context adds to Program.cs: <c>builder.Services.AddTasksContext();</c></summary>
    public static IServiceCollection AddTasksContext(this IServiceCollection services)
    {
        services.AddScoped<IStudentTaskRepository, StudentTaskRepository>();

        services.AddScoped<IStudentTaskCommandService, StudentTaskCommandService>();
        services.AddScoped<IStudentTaskQueryService, StudentTaskQueryService>();
        services.AddScoped<IVocationalProgressQueryService, VocationalProgressQueryService>();

        // TEMPORARY: replace with the real adapter over Assessments' facade when it exists.
        services.AddScoped<IAssessmentProgressProvider, StubAssessmentProgressProvider>();

        services.AddScoped<ITasksContextFacade, TasksContextFacade>();
        return services;
    }
}
