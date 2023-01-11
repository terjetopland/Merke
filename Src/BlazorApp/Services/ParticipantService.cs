using BlazorApp.Data;
using BlazorApp.Dtos;
using BlazorApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services;

public interface IParticipantService
{
    void AddParticipant(string userId, int raceId);
    string GetAll();
    void SetEndTime(int raceId, int participantId);
    List<ParticipantDto> GetParticipants(int raceId);
}

public class ParticipantService : IParticipantService
{
    private readonly AppDbContext _ctx;
    private UserManager<AppUser> _userManager;
    
    public ParticipantService(AppDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    private int AddOrGetRaceId()
    {
        var race = _ctx.Races.FirstOrDefault();
        if (race is not null) return race.Id;
        
        var newRace = new Race();
        _ctx.Add(newRace);
        _ctx.SaveChanges();
        return newRace.Id;
    }
    
    public void AddParticipant(string userId, int raceId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        var participant = new Participant
        {
            User = user,
            RaceId = raceId
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
                .ThenInclude(u => u.User)
                .FirstOrDefault(r => r.Id == raceId);

        var participantsDto = new List<ParticipantDto>();

        foreach (Participant participant in race.Participants)
        {
            participantsDto.Add(new ParticipantDto
            {
                Id = participant.Id,
                RaceId = participant.RaceId,
                EndTime = participant.EndTime,
                Name = participant.User.Name,
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