using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class TranscriptSegment : BaseEntity
{
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public int Sequence { get; set; }
    public string Speaker { get; set; } = "Unknown";
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Text { get; set; } = string.Empty;
}
