using System.Runtime.InteropServices.JavaScript;
using BlazorApp.Data;

namespace BlazorApp.Services;


public interface IRaceService
{
    void Start(int raceId);
}
public class RaceService : IRaceService
{
    private readonly AppDbContext _ctx;

    public RaceService(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public void Start(int raceId)
    {
        DateTime startTime = DateTime.UtcNow;

        var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);

        if (race is not null)
        {
            race.StartTime = startTime;
            _ctx.SaveChanges();
        }
        

    }
}