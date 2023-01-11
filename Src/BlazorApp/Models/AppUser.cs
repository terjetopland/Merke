using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Models;


public class AppUser : IdentityUser
{
    public string Name { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    
    public virtual ICollection<Participant>? Participants { get; set; }

}