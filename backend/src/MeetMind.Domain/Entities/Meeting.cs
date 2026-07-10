using MeetMind.Domain.Common;
using MeetMind.Domain.Enums;

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
    public string? TranscriptText { get; set; }
    public string? Summary { get; set; }
}
