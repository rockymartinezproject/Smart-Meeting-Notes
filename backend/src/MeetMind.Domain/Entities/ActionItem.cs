using MeetMind.Domain.Common;
using MeetMind.Domain.Enums;

namespace MeetMind.Domain.Entities;

public class ActionItem : BaseEntity
{
    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public Guid? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }
    public DateTime? Deadline { get; set; }
    public ActionItemStatus Status { get; set; } = ActionItemStatus.Todo;
    public string? SourceQuote { get; set; }
}
