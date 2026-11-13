using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;


namespace VocaFy.NextPath.Platform.Assessments.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAssessmentsConfiguration(this ModelBuilder builder)
    {
        // ---------- VocationalTest (aggregate) ----------
        builder.Entity<VocationalTest>().HasKey(t => t.Id);
        builder.Entity<VocationalTest>().Property(t => t.Id)
            .HasConversion(new StronglyTypedIdConverter<VocationalTestId, Guid>(id => id.Value,
                value => new VocationalTestId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<VocationalTest>().Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Entity<VocationalTest>().Property(t => t.ShortName).IsRequired().HasMaxLength(60);
        builder.Entity<VocationalTest>().Property(t => t.Headline).IsRequired().HasMaxLength(120);
        builder.Entity<VocationalTest>().Property(t => t.Description).IsRequired().HasMaxLength(500);
        builder.Entity<VocationalTest>().Property(t => t.Instructions).IsRequired().HasMaxLength(500);
        builder.Entity<VocationalTest>().Property(t => t.Type).HasConversion<string>().HasMaxLength(10);
        builder.Entity<VocationalTest>().Property(t => t.Version).IsRequired().HasMaxLength(10);
        builder.Entity<VocationalTest>()
            .HasMany(t => t.Questions)
            .WithOne()
            .HasForeignKey(q => q.VocationalTestId);

        // ---------- TestQuestion (entity inside VocationalTest) ----------
        builder.Entity<TestQuestion>().HasKey(q => q.Id);
        builder.Entity<TestQuestion>().Property(q => q.Id)
            .HasConversion(new StronglyTypedIdConverter<TestQuestionId, Guid>(id => id.Value,
                value => new TestQuestionId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<TestQuestion>().Property(q => q.VocationalTestId)
            .HasConversion(new StronglyTypedIdConverter<VocationalTestId, Guid>(id => id.Value,
                value => new VocationalTestId(value)))
            .IsRequired();
        builder.Entity<TestQuestion>().Property(q => q.Type).HasConversion<string>().HasMaxLength(10);
        builder.Entity<TestQuestion>().Property(q => q.Text).IsRequired().HasMaxLength(500);
        builder.Entity<TestQuestion>().Property(q => q.Area).HasConversion<string>().HasMaxLength(20);
        builder.Entity<TestQuestion>().Property(q => q.Placeholder).HasMaxLength(200);
        builder.Entity<TestQuestion>().OwnsMany(q => q.Options, o =>
        {
            o.WithOwner().HasForeignKey("TestQuestionId");
            o.Property<int>("Id");
            o.HasKey("Id");
            o.Property(p => p.Code).IsRequired().HasMaxLength(10);
            o.Property(p => p.Text).IsRequired().HasMaxLength(240);
            o.Property(p => p.Area).HasConversion<string>().HasMaxLength(20);
        });

        // ---------- TestResult (aggregate) ----------
        builder.Entity<TestResult>().HasKey(r => r.Id);
        builder.Entity<TestResult>().Property(r => r.Id)
            .HasConversion(new StronglyTypedIdConverter<TestResultId, Guid>(id => id.Value,
                value => new TestResultId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<TestResult>().Property(r => r.StudentId).IsRequired();
        builder.Entity<TestResult>().Property(r => r.VocationalTestId)
            .HasConversion(new StronglyTypedIdConverter<VocationalTestId, Guid>(id => id.Value,
                value => new VocationalTestId(value)))
            .IsRequired();
        builder.Entity<TestResult>().HasOne<VocationalTest>().WithMany().HasForeignKey(r => r.VocationalTestId);
        builder.Entity<TestResult>().Property(r => r.Version).IsRequired().HasMaxLength(10);
        builder.Entity<TestResult>().Property(r => r.Status).HasConversion<string>().HasMaxLength(15);
        builder.Entity<TestResult>().Property(r => r.Summary).HasMaxLength(120);

        builder.Entity<TestResult>().OwnsOne(r => r.Scores, s =>
        {
            s.WithOwner().HasForeignKey("Id");
            s.Property(p => p.Artistic).HasColumnName("ScoreArtistic");
            s.Property(p => p.Social).HasColumnName("ScoreSocial");
            s.Property(p => p.Investigative).HasColumnName("ScoreInvestigative");
            s.Property(p => p.Enterprising).HasColumnName("ScoreEnterprising");
            s.Property(p => p.Conventional).HasColumnName("ScoreConventional");
        });

        builder.Entity<TestResult>().OwnsMany(r => r.CareerMatches, c =>
        {
            c.WithOwner().HasForeignKey("TestResultId");
            c.Property<int>("Id");
            c.HasKey("Id");
            c.Property(p => p.CareerId).IsRequired();
            c.Property(p => p.Compatibility).IsRequired();
        });

        builder.Entity<TestResult>().OwnsMany(r => r.Answers, a =>
        {
            a.WithOwner().HasForeignKey("TestResultId");
            a.Property<int>("Id");
            a.HasKey("Id");
            a.Property(p => p.QuestionId).IsRequired();
            a.Property(p => p.Value).IsRequired().HasMaxLength(2000);
        });
    }
}
