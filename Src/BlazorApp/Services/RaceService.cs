using System.Linq.Expressions;
using BlazorApp.Data;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;


namespace BlazorApp.Services;

public interface IRaceService
{
    void StartRace(int raceId);
    void EndRace(int raceId);
    Race GetRace();

    Race GetRace(int raceId);

    List<Race> GetRaces();

    List<Race> GetOngoingRaces();
    void AddRace(string name);
    void Delete(int raceId);
}

public class RaceService : IRaceService
{
    private readonly AppDbContext _ctx;

    public RaceService(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void StartRace(int raceId)
    {
        DateTime startRace = DateTime.UtcNow;

        var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);

        if (race is not null && race.StartRace is null)
        {
            race.StartRace = startRace;
            _ctx.SaveChanges();
        }
    }

    public void EndRace(int raceId)
    {
        //Getting all participants that has not yet finished the race
        var participantsNotEndedRace = _ctx.Participants
            .Where(p => p.RaceId == raceId)
            .ToList();
        
        DateTime endTime = DateTime.UtcNow;
        var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);
        if (race is not null && race.StartRace is not null)
        {
            race.EndRace = endTime;
            _ctx.SaveChanges();
        }
        
        //Set maxvalue to all participants that has not ended race before the race ends
        foreach (var participantNotEnded in participantsNotEndedRace)
        {
            if (!participantNotEnded.EndTime.HasValue)
            {
                participantNotEnded.EndTime = DateTime.MaxValue;
                _ctx.SaveChanges();
            }
            
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
        Expression<Func<Race, bool>> raceEqual = r => r.Id == raceId;
        
        var race = _ctx.Races.FirstOrDefault(raceEqual);
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

    public List<Race> GetOngoingRaces()
    {
        Expression<Func<Race, bool>> onGoingRacesFilter = race => race.StartRace != null && race.EndRace == null;
        
        var races = _ctx.Races.Where(onGoingRacesFilter).ToList();
        return races;
    }

    public void AddRace(string name)
    {
        var newRace = new Race {Name = name};
        _ctx.Add(newRace);
        _ctx.SaveChanges();
    }

    public void Delete(int raceId)
    {
        var raceAndParticipants = _ctx.Races
            .Include(r => r.Participants)
            .FirstOrDefault(r => r.Id == raceId);
        
        // check if there are participants in race
        if (raceAndParticipants?.Participants.Count > 0)
        {
            throw new Exception($"Cannot delete a race with participants");
        }

        if (raceAndParticipants is null) return;
        
        _ctx.Races.Remove(raceAndParticipants);
        _ctx.SaveChanges();
    }
}