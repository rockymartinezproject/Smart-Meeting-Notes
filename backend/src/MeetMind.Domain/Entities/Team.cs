using MeetMind.Domain.Common;

namespace MeetMind.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? StripeCustomerId { get; set; }
    public string? StripeSubscriptionId { get; set; }
    public int? PlanId { get; set; }
    public Plan? Plan { get; set; }
    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
}
