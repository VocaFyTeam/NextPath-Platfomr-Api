using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.VocationalResources.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Maps VocationalResource to table vocational_resources (new entity added to the diagram).
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyVocationalResourcesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<VocationalResource>(resource =>
        {
            resource.HasKey(r => r.Id);
            resource.Property(r => r.Id).HasColumnName("resource_id")
                .HasConversion(new StronglyTypedIdConverter<VocationalResourceId, Guid>(id => id.Value,
                    v => new VocationalResourceId(v)))
                .ValueGeneratedNever();
            resource.Property(r => r.PsychologistId)
                .HasConversion(new StronglyTypedIdConverter<PsychologistId, Guid>(id => id.Value,
                    v => new PsychologistId(v)))
                .IsRequired();
            resource.HasIndex(r => r.PsychologistId);
            resource.Property(r => r.Title).IsRequired().HasMaxLength(150);
            resource.Property(r => r.Category).IsRequired().HasMaxLength(50);
            resource.Property(r => r.Description).HasColumnType("text");
            resource.Property(r => r.CoverUrl).HasMaxLength(255);
            resource.Property(r => r.FileUrl).IsRequired().HasMaxLength(255);
        });
    }
}
