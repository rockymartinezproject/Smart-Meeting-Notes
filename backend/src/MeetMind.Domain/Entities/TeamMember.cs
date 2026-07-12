using MeetMind.Domain.Common;
using MeetMind.Domain.Enums;

namespace MeetMind.Domain.Entities;

public class TeamMember : BaseEntity
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public TeamMemberRole Role { get; set; } = TeamMemberRole.Member;
}
