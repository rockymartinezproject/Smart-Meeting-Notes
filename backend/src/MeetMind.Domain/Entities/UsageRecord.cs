using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class UsageRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public int Year { get; set; }
    public int Month { get; set; }
    public int DurationSeconds { get; set; }
}
