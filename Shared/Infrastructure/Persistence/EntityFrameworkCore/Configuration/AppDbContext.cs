using VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using VocaFy.NextPath.Platform.VocationalResources.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Assessments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Community.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using VocaFy.NextPath.Platform.Counseling.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;


public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyIamConfiguration();

        builder.ApplyVocationalResourcesConfiguration();

        builder.ApplyAssessmentsConfiguration();
        builder.ApplyCommunityConfiguration();
        builder.ApplyCounselingConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}
