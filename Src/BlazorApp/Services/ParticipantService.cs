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
    Task SetEndTime(int participantId, int raceId);
    List<ParticipantDto> GetParticipants(int raceId);
    List<Participant> GetParticipantUsersInRace( int raceId);
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
        var userNotParticipantYet =
            _ctx.Participants
                .Include(p => p.User)
                .FirstOrDefault(p => p.RaceId == raceId && p.User!.Id == userId);

        if (userNotParticipantYet is null)
        {

            if (raceId != 0)
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

        }
    }

    public string GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task SetEndTime(int participantId, int raceId)
    {
        var participant = await _ctx.Participants.FirstOrDefaultAsync(p => p.Id == participantId);

        if (participant is not null)
        {
            participant.EndTime = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
        }
        else
        {
            throw new Exception("no endtime set!!");
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

        if (race is not null)
        {
            foreach (Participant participant in race.Participants)
            {

                participantsDto.Add(new ParticipantDto
                {
                    Id = participant.Id,
                    RaceId = participant.RaceId,
                    EndTime = participant.EndTime,
                    Name = participant.User?.Name ?? "<unknown>",
                    Result = participant.EndTime - race.StartRace
                    

                });
            }

            participantsDto.Sort((participant1, participant2) =>
                TimeSpan.Compare(
                    participant1.Result ?? new TimeSpan(0),
                    participant2.Result ?? new TimeSpan(0)));
        }


        return participantsDto;
    }
    
    //This is maybe not DRY, but now I figured out how to get currentParticipant and hide button 'Add' if user was already participant
    //VillereV maybe you have a better solution for this.
    public List<Participant> GetParticipantUsersInRace( int raceId)
    {
        List<Participant> usersInRace = _ctx.Participants
            .Include(p => p.User)
            .Where(p=> p.RaceId == raceId)
            .ToList();
        

        return usersInRace;
    }
    
}