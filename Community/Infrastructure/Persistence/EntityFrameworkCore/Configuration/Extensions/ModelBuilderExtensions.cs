using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Community.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCommunityConfiguration(this ModelBuilder builder)
    {
        // ---------- InterestCommunity (aggregate) -> table interest_communities ----------
        builder.Entity<InterestCommunity>().HasKey(c => c.Id);
        builder.Entity<InterestCommunity>().Property(c => c.Id)
            .HasConversion(new StronglyTypedIdConverter<CommunityId, Guid>(id => id.Value,
                value => new CommunityId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<InterestCommunity>().Property(c => c.Name).IsRequired().HasMaxLength(80);
        builder.Entity<InterestCommunity>().HasIndex(c => c.Name).IsUnique();
        builder.Entity<InterestCommunity>().Property(c => c.Description).HasMaxLength(300);
        builder.Entity<InterestCommunity>().OwnsMany(c => c.Members, m =>
        {
            m.WithOwner().HasForeignKey("CommunityId");
            m.Property<int>("Id");
            m.HasKey("Id");
            m.Property(p => p.StudentId).IsRequired();
            m.Property(p => p.JoinedAt).IsRequired();
        });

        // ---------- ForumThread (aggregate) -> table forum_threads ----------
        builder.Entity<ForumThread>().HasKey(t => t.Id);
        builder.Entity<ForumThread>().Property(t => t.Id)
            .HasConversion(new StronglyTypedIdConverter<ForumThreadId, Guid>(id => id.Value,
                value => new ForumThreadId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<ForumThread>().Property(t => t.CommunityId)
            .HasConversion(new StronglyTypedIdConverter<CommunityId, Guid>(id => id.Value,
                value => new CommunityId(value)))
            .IsRequired();
        builder.Entity<ForumThread>().HasOne<InterestCommunity>().WithMany().HasForeignKey(t => t.CommunityId);
        builder.Entity<ForumThread>().Property(t => t.AuthorId).IsRequired();
        builder.Entity<ForumThread>().Property(t => t.AuthorName).IsRequired().HasMaxLength(80);
        builder.Entity<ForumThread>().Property(t => t.Content).IsRequired().HasMaxLength(2000);
        builder.Entity<ForumThread>().Property(t => t.ReportReason).HasMaxLength(300);

        // ---------- Comment (aggregate) -> table comments ----------
        builder.Entity<Comment>().HasKey(c => c.Id);
        builder.Entity<Comment>().Property(c => c.Id)
            .HasConversion(new StronglyTypedIdConverter<CommentId, Guid>(id => id.Value,
                value => new CommentId(value)))
            .IsRequired()
            .ValueGeneratedNever();
        builder.Entity<Comment>().Property(c => c.ForumThreadId)
            .HasConversion(new StronglyTypedIdConverter<ForumThreadId, Guid>(id => id.Value,
                value => new ForumThreadId(value)))
            .IsRequired();
        builder.Entity<Comment>().HasOne<ForumThread>().WithMany().HasForeignKey(c => c.ForumThreadId);
        builder.Entity<Comment>().Property(c => c.AuthorName).IsRequired().HasMaxLength(80);
        builder.Entity<Comment>().Property(c => c.Content).IsRequired().HasMaxLength(1000);
    }
}
