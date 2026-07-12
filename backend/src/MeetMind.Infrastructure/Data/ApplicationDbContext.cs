using MeetMind.Domain.Entities;
using MeetMind.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

namespace MeetMind.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<TranscriptSegment> TranscriptSegments => Set<TranscriptSegment>();
    public DbSet<Summary> Summaries => Set<Summary>();
    public DbSet<ActionItem> ActionItems => Set<ActionItem>();
    public DbSet<KeyDecision> KeyDecisions => Set<KeyDecision>();
    public DbSet<TopicSegment> TopicSegments => Set<TopicSegment>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresExtension("vector");

        ConfigureIdentity(builder);
        ConfigureMeeting(builder);
        ConfigureTranscriptSegment(builder);
        ConfigureSummary(builder);
        ConfigureActionItem(builder);
        ConfigureKeyDecision(builder);
        ConfigureTopicSegment(builder);
        ConfigureComment(builder);
        ConfigureTeam(builder);
        ConfigureTeamMember(builder);
        ConfigurePlan(builder);
        ConfigureUsageRecord(builder);
    }

    private static void ConfigureIdentity(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasMany(u => u.Meetings)
                .WithOne(m => m.Owner)
                .HasForeignKey(m => m.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.TeamMemberships)
                .WithOne(tm => tm.User)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.AssignedActionItems)
                .WithOne(ai => ai.Owner)
                .HasForeignKey(ai => ai.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(u => u.Comments)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureMeeting(ModelBuilder builder)
    {
        builder.Entity<Meeting>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.OwnerId);
            entity.HasIndex(m => m.TeamId);
            entity.HasIndex(m => m.Status);
            entity.HasIndex(m => m.CreatedAt);

            entity.Property(m => m.Status)
                .HasConversion<string>();

            entity.Property(m => m.Embedding)
                .HasColumnType("vector(1536)");

            entity.HasIndex(m => m.Embedding)
                .HasMethod("hnsw")
                .HasOperators("vector_cosine_ops");

            entity.Property(m => m.TranscriptText)
                .HasColumnType("text");

            entity.Property<NpgsqlTsVector>("SearchVector")
                .HasComputedColumnSql(
                    "to_tsvector('english', coalesce(\"Title\", '') || ' ' || coalesce(\"TranscriptText\", ''))",
                    stored: true);

            entity.HasIndex("SearchVector")
                .HasMethod("GIN");

            entity.HasOne(m => m.Team)
                .WithMany(t => t.Meetings)
                .HasForeignKey(m => m.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    private static void ConfigureTranscriptSegment(ModelBuilder builder)
    {
        builder.Entity<TranscriptSegment>(entity =>
        {
            entity.HasKey(ts => ts.Id);
            entity.HasIndex(ts => ts.MeetingId);
            entity.HasIndex(ts => ts.Sequence);

            entity.Property(ts => ts.Text)
                .HasColumnType("text");
        });
    }

    private static void ConfigureSummary(ModelBuilder builder)
    {
        builder.Entity<Summary>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.MeetingId)
                .IsUnique();

            entity.Property(s => s.Content)
                .HasColumnType("text");
        });
    }

    private static void ConfigureActionItem(ModelBuilder builder)
    {
        builder.Entity<ActionItem>(entity =>
        {
            entity.HasKey(ai => ai.Id);
            entity.HasIndex(ai => ai.MeetingId);
            entity.HasIndex(ai => ai.OwnerId);
            entity.HasIndex(ai => ai.Status);
            entity.HasIndex(ai => ai.Deadline);

            entity.Property(ai => ai.Status)
                .HasConversion<string>();
        });
    }

    private static void ConfigureKeyDecision(ModelBuilder builder)
    {
        builder.Entity<KeyDecision>(entity =>
        {
            entity.HasKey(kd => kd.Id);
            entity.HasIndex(kd => kd.MeetingId);
        });
    }

    private static void ConfigureTopicSegment(ModelBuilder builder)
    {
        builder.Entity<TopicSegment>(entity =>
        {
            entity.HasKey(ts => ts.Id);
            entity.HasIndex(ts => ts.MeetingId);
        });
    }

    private static void ConfigureComment(ModelBuilder builder)
    {
        builder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.MeetingId);
            entity.HasIndex(c => c.AuthorId);
            entity.HasIndex(c => c.ParentCommentId);

            entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTeam(ModelBuilder builder)
    {
        builder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.Slug)
                .IsUnique();

            entity.HasOne(t => t.Plan)
                .WithMany()
                .HasForeignKey(t => t.PlanId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    private static void ConfigureTeamMember(ModelBuilder builder)
    {
        builder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(tm => tm.Id);
            entity.HasIndex(tm => new { tm.TeamId, tm.UserId })
                .IsUnique();

            entity.Property(tm => tm.Role)
                .HasConversion<string>();
        });
    }

    private static void ConfigurePlan(ModelBuilder builder)
    {
        builder.Entity<Plan>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Name)
                .IsUnique();
            entity.HasIndex(p => p.StripePriceId)
                .IsUnique();

            entity.HasData(
                new Plan
                {
                    Id = 1,
                    Name = "Free",
                    StripePriceId = "price_free",
                    MonthlyPrice = 0m,
                    MonthlyMeetingLimit = 3,
                    MaxMeetingDurationMinutes = 30,
                    HasFullSummary = false,
                    HasSemanticSearch = false,
                    HasTeamWorkspace = false,
                    HasAnalytics = false
                },
                new Plan
                {
                    Id = 2,
                    Name = "Pro",
                    StripePriceId = "price_pro",
                    MonthlyPrice = 15m,
                    MonthlyMeetingLimit = 20,
                    MaxMeetingDurationMinutes = 120,
                    HasFullSummary = true,
                    HasSemanticSearch = true,
                    HasTeamWorkspace = false,
                    HasAnalytics = false
                },
                new Plan
                {
                    Id = 3,
                    Name = "Team",
                    StripePriceId = "price_team",
                    MonthlyPrice = 49m,
                    MonthlyMeetingLimit = int.MaxValue,
                    MaxMeetingDurationMinutes = 120,
                    HasFullSummary = true,
                    HasSemanticSearch = true,
                    HasTeamWorkspace = true,
                    HasAnalytics = true
                },
                new Plan
                {
                    Id = 4,
                    Name = "Enterprise",
                    StripePriceId = "price_enterprise",
                    MonthlyPrice = 0m,
                    MonthlyMeetingLimit = int.MaxValue,
                    MaxMeetingDurationMinutes = int.MaxValue,
                    HasFullSummary = true,
                    HasSemanticSearch = true,
                    HasTeamWorkspace = true,
                    HasAnalytics = true
                });
        });
    }

    private static void ConfigureUsageRecord(ModelBuilder builder)
    {
        builder.Entity<UsageRecord>(entity =>
        {
            entity.HasKey(ur => ur.Id);
            entity.HasIndex(ur => new { ur.UserId, ur.Year, ur.Month });
            entity.HasIndex(ur => ur.MeetingId);
        });
    }
}
