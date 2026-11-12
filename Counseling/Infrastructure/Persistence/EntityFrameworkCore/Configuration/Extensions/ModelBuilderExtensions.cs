using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Counseling.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Maps the sessions table of the database diagram (session_id, psychologist_id, student_id, session_type,
///     scheduled_date_time, duration_minutes, status, notes) with table-per-hierarchy: IndividualSession and
///     GroupSession share it and session_type is the discriminator. Group participants go to session_participants.
/// </summary>
public static class ModelBuilderExtensions
{
    public static void ApplyCounselingConfiguration(this ModelBuilder builder)
    {
        var sessionIdConverter = new StronglyTypedIdConverter<SessionId, Guid>(id => id.Value, v => new SessionId(v));
        var studentIdConverter = new StronglyTypedIdConverter<StudentId, Guid>(id => id.Value, v => new StudentId(v));

        builder.Entity<Session>(session =>
        {
            session.UseTphMappingStrategy();
            session.HasKey(s => s.Id);
            session.Property(s => s.Id).HasColumnName("session_id")
                .HasConversion(sessionIdConverter)
                .ValueGeneratedNever();
            session.HasDiscriminator<string>("SessionType")
                .HasValue<IndividualSession>("INDIVIDUAL")
                .HasValue<GroupSession>("GROUP");
            session.Property<string>("SessionType").HasMaxLength(20);

            session.Property(s => s.PsychologistId)
                .HasConversion(new StronglyTypedIdConverter<PsychologistId, Guid>(id => id.Value,
                    v => new PsychologistId(v)))
                .IsRequired();
            session.HasIndex(s => s.PsychologistId);
            session.Property(s => s.Title).IsRequired().HasMaxLength(150);
            // The agenda keeps wall-clock time (what the psychologist typed). It is stored tagged as UTC so
            // providers that require a kind (Npgsql: timestamptz) accept it, and read back as unspecified.
            session.Property(s => s.ScheduledDateTime).IsRequired()
                .HasConversion(v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified));
            session.HasIndex(s => s.ScheduledDateTime);
            session.Property(s => s.DurationMinutes).IsRequired();
            session.Property(s => s.Status).HasConversion<string>().IsRequired().HasMaxLength(20);
            session.Property(s => s.Notes).HasColumnType("text");
            session.Property(s => s.MeetLink).HasMaxLength(255);

            // Computed in the domain, not stored.
            session.Ignore(s => s.Type);
            session.Ignore(s => s.StudentIds);
            session.Ignore(s => s.EndDateTime);
            session.Ignore(s => s.IsScheduled);
        });

        builder.Entity<IndividualSession>(individual =>
        {
            individual.Property(s => s.StudentId).HasConversion(studentIdConverter);
            individual.HasIndex(s => s.StudentId);
        });

        builder.Entity<GroupSession>(group =>
        {
            group.Property(g => g.GroupId).HasMaxLength(50);
            group.Property(g => g.GroupingCriteria).HasMaxLength(255);
            group.Property(g => g.MaxParticipants);
            group.Ignore(g => g.IsFull);

            group.OwnsMany(g => g.Participants, participant =>
            {
                participant.ToTable("session_participants");
                participant.Property<SessionId>("SessionId").HasConversion(sessionIdConverter);
                participant.WithOwner().HasForeignKey("SessionId");
                participant.Property(p => p.StudentId).HasConversion(studentIdConverter).IsRequired();
                participant.HasKey("SessionId", nameof(SessionParticipant.StudentId));
                participant.HasIndex(p => p.StudentId);
            });
            group.Navigation(g => g.Participants).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
