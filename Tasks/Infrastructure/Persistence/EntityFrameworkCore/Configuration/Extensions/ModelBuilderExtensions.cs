using Microsoft.EntityFrameworkCore;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>Call it from <c>AppDbContext.OnModelCreating</c>, before <c>UseSnakeCaseNamingConvention()</c>.</summary>
    public static void ApplyTasksConfiguration(this ModelBuilder builder)
    {
        builder.Entity<StudentTask>().HasKey(t => t.Id);
        builder.Entity<StudentTask>().Property(t => t.Id)
            .HasConversion(new StronglyTypedIdConverter<StudentTaskId, int>(id => id.Value,
                value => new StudentTaskId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<StudentTask>().Property(t => t.StudentId)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(id => id.Value, value => new StudentId(value)))
            .IsRequired();
        builder.Entity<StudentTask>().Property(t => t.Title).IsRequired().HasMaxLength(StudentTask.MaxTitleLength);
        builder.Entity<StudentTask>().Property(t => t.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
        builder.Entity<StudentTask>().Property(t => t.DueDate);

        // "order" is a reserved word in SQL, so the column gets a safe (already snake_case) name.
        builder.Entity<StudentTask>().Property(t => t.Order).HasColumnName("display_order").IsRequired();

        // StudentId has no foreign key: students live in the IAM context.
        builder.Entity<StudentTask>().HasIndex(t => new { t.StudentId, t.Order });
    }
}
