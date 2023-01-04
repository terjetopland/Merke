namespace BlazorApp.Models;

public class Race
{
    public int Id { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public DateTime? StartTime { get; set; } 

    public virtual ICollection<Participant> Participants { get; set; } = new HashSet<Participant>();
    
}