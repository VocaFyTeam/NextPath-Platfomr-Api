using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        var userIdConverter = new StronglyTypedIdConverter<UserId, Guid>(id => id.Value, value => new UserId(value));

        // users
        builder.Entity<User>(user =>
        {
            user.HasKey(u => u.Id);
            user.Property(u => u.Id).HasColumnName("user_id").HasConversion(userIdConverter).ValueGeneratedNever();
            user.Property(u => u.Email).IsRequired().HasMaxLength(255);
            user.HasIndex(u => u.Email).IsUnique();
            user.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            user.Property(u => u.Role).HasConversion<string>().IsRequired().HasMaxLength(20);
            user.Property(u => u.IsSessionActive).IsRequired();
            user.Property(u => u.CreatedAt).IsRequired();
        });

        builder.Entity<Student>(student =>
        {
            student.HasKey(s => s.Id);
            student.Property(s => s.Id).HasColumnName("student_id")
                .HasConversion(new StronglyTypedIdConverter<StudentId, Guid>(id => id.Value, v => new StudentId(v)))
                .ValueGeneratedNever();
            student.Property(s => s.UserId).HasConversion(userIdConverter).IsRequired();
            student.HasIndex(s => s.UserId).IsUnique();
            student.HasOne<User>().WithOne().HasForeignKey<Student>(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
            student.Property(s => s.SchoolOrUniversity).IsRequired().HasMaxLength(255);
            student.Property(s => s.CurrentCycle).IsRequired().HasMaxLength(50);
            student.Property(s => s.LinkedPsychologistCode).HasMaxLength(50);
        });

        builder.Entity<Psychologist>(psychologist =>
        {
            psychologist.HasKey(p => p.Id);
            psychologist.Property(p => p.Id).HasColumnName("psychologist_id")
                .HasConversion(new StronglyTypedIdConverter<PsychologistId, Guid>(id => id.Value,
                    v => new PsychologistId(v)))
                .ValueGeneratedNever();
            psychologist.Property(p => p.UserId).HasConversion(userIdConverter).IsRequired();
            psychologist.HasIndex(p => p.UserId).IsUnique();
            psychologist.HasOne<User>().WithOne().HasForeignKey<Psychologist>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            psychologist.Property(p => p.ProfessionalLicense).IsRequired().HasMaxLength(100);
            psychologist.Property(p => p.Specialty).IsRequired().HasMaxLength(100);
            psychologist.Property(p => p.UniqueLinkingCode).IsRequired().HasMaxLength(50);
            psychologist.HasIndex(p => p.UniqueLinkingCode).IsUnique();
        });

        builder.Entity<PasswordResetToken>(token =>
        {
            token.HasKey(t => t.Id);
            token.Property(t => t.Id).HasColumnName("token_id")
                .HasConversion(new StronglyTypedIdConverter<PasswordResetTokenId, Guid>(id => id.Value,
                    v => new PasswordResetTokenId(v)))
                .ValueGeneratedNever();
            token.Property(t => t.UserId).HasConversion(userIdConverter).IsRequired();
            token.HasOne<User>().WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
            token.Property(t => t.Token).IsRequired().HasMaxLength(255);
            token.HasIndex(t => t.Token).IsUnique();
            token.Property(t => t.ExpirationDate).IsRequired();
            token.Property(t => t.IsUsed).IsRequired();
        });
    }
}
