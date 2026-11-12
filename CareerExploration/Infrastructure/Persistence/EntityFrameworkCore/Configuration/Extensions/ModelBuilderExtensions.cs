using Microsoft.EntityFrameworkCore;
using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace NextPath.Platform.CareerExploration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>Call it from <c>AppDbContext.OnModelCreating</c>, before <c>UseSnakeCaseNamingConvention()</c>.</summary>
    public static void ApplyCareerExplorationConfiguration(this ModelBuilder builder)
    {
        // ---------- Career ----------
        builder.Entity<Career>().HasKey(c => c.Id);
        builder.Entity<Career>().Property(c => c.Id)
            .HasConversion(new StronglyTypedIdConverter<CareerId, int>(id => id.Value, value => new CareerId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Career>().Property(c => c.Name).IsRequired().HasMaxLength(120);
        builder.Entity<Career>().Property(c => c.Emoji).IsRequired().HasMaxLength(16);
        builder.Entity<Career>().Property(c => c.Area).IsRequired().HasMaxLength(100);
        builder.Entity<Career>().Property(c => c.ImageUrl).IsRequired().HasMaxLength(300);
        builder.Entity<Career>().Property(c => c.ShortDescription).IsRequired().HasMaxLength(300);
        builder.Entity<Career>().Property(c => c.Description).IsRequired().HasMaxLength(2000);
        builder.Entity<Career>().Property(c => c.DurationLabel).IsRequired().HasMaxLength(60);
        builder.Entity<Career>().Property(c => c.Modality).IsRequired().HasMaxLength(50);
        builder.Entity<Career>().Property(c => c.ModalityDetail).IsRequired().HasMaxLength(100);
        builder.Entity<Career>().Property(c => c.MainField).IsRequired().HasMaxLength(200);
        builder.Entity<Career>().Property(c => c.DemandLevel).IsRequired().HasMaxLength(30);
        builder.Entity<Career>().Property(c => c.Universities).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.FieldOfWork).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.Skills).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.AverageSalary).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.SalaryRange).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.Profile).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.Projection).HasJsonConversion().IsRequired();
        builder.Entity<Career>().Property(c => c.Recommendations).HasJsonConversion().IsRequired();
        builder.Entity<Career>().HasIndex(c => c.Name).IsUnique();
        builder.Entity<Career>().HasIndex(c => c.Area);

        // ---------- FavoriteCareer ----------
        builder.Entity<FavoriteCareer>().HasKey(f => f.Id);
        builder.Entity<FavoriteCareer>().Property(f => f.Id)
            .HasConversion(new StronglyTypedIdConverter<FavoriteCareerId, int>(id => id.Value,
                value => new FavoriteCareerId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<FavoriteCareer>().Property(f => f.StudentId)
            .HasConversion(new StronglyTypedIdConverter<StudentId, int>(id => id.Value, value => new StudentId(value)))
            .IsRequired();
        builder.Entity<FavoriteCareer>().Property(f => f.CareerId)
            .HasConversion(new StronglyTypedIdConverter<CareerId, int>(id => id.Value, value => new CareerId(value)))
            .IsRequired();

        // A student cannot save the same career twice (HU19).
        builder.Entity<FavoriteCareer>().HasIndex(f => new { f.StudentId, f.CareerId }).IsUnique();

        // Same bounded context, so a real foreign key is allowed. StudentId has NO foreign key:
        // students live in the IAM context.
        builder.Entity<FavoriteCareer>()
            .HasOne<Career>()
            .WithMany()
            .HasForeignKey(f => f.CareerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
