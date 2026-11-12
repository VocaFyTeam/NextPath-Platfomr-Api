using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    private static StronglyTypedIdConverter<TId, int> IdConverter<TId>(Func<int, TId> factory)
        where TId : IEntityId<int>
    {
        return new StronglyTypedIdConverter<TId, int>(id => id.Value, factory);
    }

    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        // ---------------- Student Monitoring Context ----------------

        // StudentGroup
        builder.Entity<StudentGroup>().HasKey(g => g.Id);
        builder.Entity<StudentGroup>().Property(g => g.Id)
            .HasConversion(IdConverter(value => new StudentGroupId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<StudentGroup>().Property(g => g.PsychologistId)
            .HasConversion(IdConverter(value => new PsychologistId(value)))
            .IsRequired();
        builder.Entity<StudentGroup>().Property(g => g.Name).IsRequired().HasMaxLength(StudentGroup.MaxNameLength);
        builder.Entity<StudentGroup>().Property(g => g.Description).IsRequired()
            .HasMaxLength(StudentGroup.MaxDescriptionLength);
        builder.Entity<StudentGroup>().HasIndex(g => new { g.PsychologistId, g.Name }).IsUnique();

        // HistoricalProfile
        builder.Entity<HistoricalProfile>().HasKey(p => p.Id);
        builder.Entity<HistoricalProfile>().Property(p => p.Id)
            .HasConversion(IdConverter(value => new HistoricalProfileId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<HistoricalProfile>().Property(p => p.StudentId)
            .HasConversion(IdConverter(value => new StudentId(value)))
            .IsRequired();
        builder.Entity<HistoricalProfile>().Property(p => p.AssignedPsychologistId)
            .HasConversion(IdConverter(value => new PsychologistId(value)))
            .IsRequired();
        builder.Entity<HistoricalProfile>().Property(p => p.GroupId)
            .HasConversion(IdConverter(value => new StudentGroupId(value)));
        builder.Entity<HistoricalProfile>().Property(p => p.ObservationSummary).IsRequired()
            .HasMaxLength(HistoricalProfile.MaxObservationSummaryLength);
        builder.Entity<HistoricalProfile>().Property(p => p.Flags)
            .HasConversion(CsvConversions.CreateFlagsConverter(), CsvConversions.CreateFlagsComparer())
            .IsRequired()
            .HasMaxLength(64);
        builder.Entity<HistoricalProfile>().Property(p => p.LastActivityAt).IsRequired();
        builder.Entity<HistoricalProfile>().HasIndex(p => p.StudentId).IsUnique();
        builder.Entity<HistoricalProfile>().HasIndex(p => p.AssignedPsychologistId);
        builder.Entity<HistoricalProfile>().HasOne<StudentGroup>()
            .WithMany()
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<ProfessionalObservation>().HasKey(o => o.Id);
        builder.Entity<ProfessionalObservation>().Property(o => o.Id)
            .HasConversion(IdConverter(value => new ProfessionalObservationId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ProfessionalObservation>().Property(o => o.HistoricalProfileId)
            .HasConversion(IdConverter(value => new HistoricalProfileId(value)))
            .IsRequired();
        builder.Entity<ProfessionalObservation>().Property(o => o.StudentId)
            .HasConversion(IdConverter(value => new StudentId(value)))
            .IsRequired();
        builder.Entity<ProfessionalObservation>().Property(o => o.PsychologistId)
            .HasConversion(IdConverter(value => new PsychologistId(value)))
            .IsRequired();
        builder.Entity<ProfessionalObservation>().Property(o => o.ObservationText).IsRequired()
            .HasMaxLength(ProfessionalObservation.MaxObservationLength);
        builder.Entity<ProfessionalObservation>().Property(o => o.Agreements).IsRequired()
            .HasMaxLength(ProfessionalObservation.MaxAgreementsLength);
        builder.Entity<ProfessionalObservation>().Property(o => o.DigitalSignature).IsRequired().HasMaxLength(64);
        builder.Entity<HistoricalProfile>().HasMany(p => p.Observations)
            .WithOne()
            .HasForeignKey(o => o.HistoricalProfileId)
            .HasConstraintName("fk_prof_observations_profile")
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------- Reports Context ----------------

        // VocationalReport
        builder.Entity<VocationalReport>().HasKey(r => r.Id);
        builder.Entity<VocationalReport>().Property(r => r.Id)
            .HasConversion(IdConverter(value => new VocationalReportId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<VocationalReport>().Property(r => r.StudentId)
            .HasConversion(IdConverter(value => new StudentId(value)))
            .IsRequired();
        builder.Entity<VocationalReport>().Property(r => r.PsychologistId)
            .HasConversion(IdConverter(value => new PsychologistId(value)))
            .IsRequired();
        builder.Entity<VocationalReport>().Property(r => r.ReportType).HasConversion<string>().IsRequired()
            .HasMaxLength(30);
        builder.Entity<VocationalReport>().Property(r => r.ContentSummary).IsRequired()
            .HasMaxLength(VocationalReport.MaxContentSummaryLength);
        builder.Entity<VocationalReport>().Property(r => r.IsValidatedByPsychologist).IsRequired();
        builder.Entity<VocationalReport>().HasIndex(r => r.StudentId);
        builder.Entity<VocationalReport>().HasIndex(r => r.PsychologistId);

        // GroupComparisonReport
        builder.Entity<GroupComparisonReport>().HasKey(r => r.Id);
        builder.Entity<GroupComparisonReport>().Property(r => r.Id)
            .HasConversion(IdConverter(value => new GroupComparisonReportId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<GroupComparisonReport>().Property(r => r.PsychologistId)
            .HasConversion(IdConverter(value => new PsychologistId(value)))
            .IsRequired();
        builder.Entity<GroupComparisonReport>().Property(r => r.SelectedGroupIds)
            .HasConversion(CsvConversions.CreateGroupIdsConverter(), CsvConversions.CreateGroupIdsComparer())
            .IsRequired()
            .HasMaxLength(500);
        builder.Entity<GroupComparisonReport>().Property(r => r.AggregatedConclusions).IsRequired()
            .HasMaxLength(GroupComparisonReport.MaxConclusionsLength);
        builder.Entity<GroupComparisonReport>().HasIndex(r => r.PsychologistId);
    }
}
