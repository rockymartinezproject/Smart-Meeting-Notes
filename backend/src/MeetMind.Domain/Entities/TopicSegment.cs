using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class TopicSegment : BaseEntity
{
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Summary { get; set; }
}
