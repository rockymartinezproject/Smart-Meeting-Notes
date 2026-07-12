using MeetMind.Domain.Common;
using MeetMind.Domain.Enums;
using Pgvector;

namespace MeetMind.Domain.Entities;

public class Meeting : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public string? OriginalFileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public MeetingStatus Status { get; set; } = MeetingStatus.Uploaded;
    public int? DurationSeconds { get; set; }
    public Guid OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = null!;
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }

    public string? TranscriptText { get; set; }
    public Summary? Summary { get; set; }
    public Vector? Embedding { get; set; }

    public ICollection<TranscriptSegment> TranscriptSegments { get; set; } = new List<TranscriptSegment>();
    public ICollection<ActionItem> ActionItems { get; set; } = new List<ActionItem>();
    public ICollection<KeyDecision> KeyDecisions { get; set; } = new List<KeyDecision>();
    public ICollection<TopicSegment> TopicSegments { get; set; } = new List<TopicSegment>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>();
}
