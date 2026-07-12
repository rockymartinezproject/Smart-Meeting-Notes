using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class Summary : BaseEntity
{
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
}
