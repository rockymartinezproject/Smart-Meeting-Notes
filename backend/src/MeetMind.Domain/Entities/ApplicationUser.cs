using Microsoft.AspNetCore.Identity;

namespace MeetMind.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public string SubscriptionTier { get; set; } = "Free";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
