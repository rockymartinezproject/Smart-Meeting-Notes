using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class KeyDecision : BaseEntity
{
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public string Decision { get; set; } = string.Empty;
    public string? ContextQuote { get; set; }
}
