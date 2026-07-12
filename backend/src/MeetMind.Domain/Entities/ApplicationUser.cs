using Microsoft.AspNetCore.Identity;

namespace MeetMind.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public int? PlanId { get; set; }
    public Plan? Plan { get; set; }
    public Guid? PersonalTeamId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    public ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
    public ICollection<ActionItem> AssignedActionItems { get; set; } = new List<ActionItem>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
