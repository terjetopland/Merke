namespace BlazorApp.Models;

public class Participant 
{
    public int  Id { get; set; }
    public int RaceId { get; set; }
    public string? UserId { get; set; }
    public DateTime? EndTime { get; set; }
    public virtual AppUser? User { get; set; }
    public virtual Race Race { get; set; } = default!;
}