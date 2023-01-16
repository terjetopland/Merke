using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Dtos;

public class ParticipantDto
{
    public int Id { get; set; }
    public int RaceId { get; set; }
    
    public DateTime? EndTime { get; set; }
    public string Name { get; set; } = default!;
    
    public TimeSpan? Result { get; set; }
    
    public string? Result2 { get; set; }
}