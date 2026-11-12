using VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using VocaFy.NextPath.Platform.VocationalResources.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context shared by every bounded context of NextPath.
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // IAM Context
        builder.ApplyIamConfiguration();

        // Vocational Resources Context
        builder.ApplyVocationalResourcesConfiguration();

        // (Each teammate adds here the ApplyXxxConfiguration() of their bounded context)

        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}
