using BlazorApp.Data;
using BlazorApp.Dtos;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services;

public interface IParticipantService
{
    void Add(string name);
    string GetAll();
    void SetEndTime(int raceId, int participantId);
    List<ParticipantDto> GetParticipants(int raceId);
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

    public void SetEndTime(int raceId, int participantId)
    {
        var participant = _ctx.Participants.FirstOrDefault(p => p.Id == participantId && p.RaceId == raceId);

        if (participant is not null && participant.EndTime is null)
        {
            participant.EndTime = DateTime.UtcNow;
            _ctx.SaveChanges();
        }
    }

    public List<ParticipantDto> GetParticipants(int raceId)
    {
        //var participants = _ctx.Participants.Where(p => p.RaceId == raceId).ToList();
        //var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);

        var race =
            _ctx.Races
                .Include(r => r.Participants)
                .FirstOrDefault(r => r.Id == raceId);

        var participantsDto = new List<ParticipantDto>();

        foreach (Participant participant in race.Participants)
        {
            participantsDto.Add(new ParticipantDto
            {
                Id = participant.Id,
                RaceId = participant.RaceId,
                EndTime = participant.EndTime,
                Name = participant.Name,
                Result = participant.EndTime - race.StartTime 
            });
        }
        participantsDto.Sort((participant1, participant2)=>
            TimeSpan.Compare(
                participant1.Result ?? new TimeSpan(0),
                participant2.Result ?? new TimeSpan(0)));

      
        return participantsDto;
        
    }
}