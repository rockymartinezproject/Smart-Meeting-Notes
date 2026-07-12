namespace MeetMind.Domain.Entities;

public class Plan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StripePriceId { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public int MonthlyMeetingLimit { get; set; }
    public int MaxMeetingDurationMinutes { get; set; }
    public bool HasFullSummary { get; set; }
    public bool HasSemanticSearch { get; set; }
    public bool HasTeamWorkspace { get; set; }
    public bool HasAnalytics { get; set; }
}
