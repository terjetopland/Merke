using System.Runtime.InteropServices.JavaScript;
using BlazorApp.Data;
using BlazorApp.Models;

namespace BlazorApp.Services;


public interface IRaceService
{
    void Start(int raceId);
    Race GetRace();

    Race GetRace(int raceId);

    List<Race> GetRaces();
    void AddRace(string name);
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

        if (race is not null && race.StartTime is null)
        {
            race.StartTime = startTime;
            _ctx.SaveChanges();
        }
    }

    public Race GetRace()
    {
        var race = _ctx.Races.FirstOrDefault();

        if (race is not null)
        {
            return race;
        }

        var newRace = new Race();
        //should this be added to db?
        _ctx.Races.Add(newRace);
        _ctx.SaveChanges();

        return newRace;
        

    }

    public Race GetRace(int raceId)
    {
        var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);
        if (race is not null)
        {
            return race;
        }

        throw new Exception("Failed to GetRace in RaceService.cs");
    }

    public List<Race> GetRaces()
    {
        var races = _ctx.Races.ToList();
        return races;
    }

    public void AddRace(string name)
    {
        var newRace = new Race{Name = name};
        _ctx.Add(newRace);
        _ctx.SaveChanges();
    }
}