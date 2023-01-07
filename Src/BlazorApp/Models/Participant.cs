namespace BlazorApp.Models;

public class Participant 
{
    public int  Id { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public virtual User? User { get; set; } = default!;

    public int RaceId { get; set; }
    public virtual Race Race { get; set; } = default!;
}