using BlazorApp.Data;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IParticipantService
{
    void Add(string name);
    string GetAll();
}

public class ParticipantService : IParticipantService
{
    private readonly AppDbContext _ctx;

    private int AddOrGetRaceId()
    {
        var race = _ctx.Races.FirstOrDefault();
        if (race is not null) return race.Id;
        
        var newRace = new Race();
        _ctx.Add(newRace);
        _ctx.SaveChanges();
        return newRace.Id;
    }
    
    public ParticipantService(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public void Add(string name)
    {
        var participant = new Participant
        {
            Name = name,
            RaceId = AddOrGetRaceId()
        };
        _ctx.Participants.Add(participant);
        _ctx.SaveChanges();
    }

    public string GetAll()
    {
        throw new NotImplementedException();
    }
}