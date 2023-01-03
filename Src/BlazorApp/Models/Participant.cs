namespace BlazorApp.Models;

public class Participant
{
    public int  Id { get; set; }
    public string Name { get; set; } = default!;

    public int RaceId { get; set; }
    public virtual Race Race { get; set; } = default!;
}