namespace BlazorApp.Models;

public class Race
{
    public int Id { get; set; }
    public string? Name { get; set; }  
    public string? Info { get; set; }
    public DateTime? StartTime { get; set; } //Planned time for race to start
    public DateTime? StartRace { get; set; }
    public DateTime? EndRace { get; set; }
    public virtual ICollection<Participant> Participants { get; set; } = new HashSet<Participant>();
}