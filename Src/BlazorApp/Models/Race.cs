namespace BlazorApp.Models;

public class Race
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // maybe property can be nullable instead?
    public string Info { get; set; } = string.Empty; // maybe property can be nullable instead?
    public DateTime? StartTime { get; set; }
    public DateTime? StartRace { get; set; }
    public DateTime? EndRace { get; set; }
    public virtual ICollection<Participant> Participants { get; set; } = new HashSet<Participant>();
}