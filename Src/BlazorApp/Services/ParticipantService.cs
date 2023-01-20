using BlazorApp.Data;
using BlazorApp.Dtos;
using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services;

public interface IParticipantService
{
    void AddParticipant(string userId, int raceId);
    void DeleteParticipant(int participantId, int raceId);
    Task SetEndTime(int participantId, int raceId);
    List<ParticipantDto> GetParticipants(int raceId);
}

public class ParticipantService : IParticipantService
{
    private readonly AppDbContext _ctx;

    public ParticipantService(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public void AddParticipant(string userId, int raceId)
    {
        if (raceId == 0) return;
        
        var userNotParticipantYet =
            _ctx.Participants
                .Include(p => p.User)
                .FirstOrDefault(p => p.RaceId == raceId && p.User!.Id == userId);

        if (userNotParticipantYet is not null) return;

        var participant = new Participant
        {
            UserId = userId,
            RaceId = raceId
        };
        _ctx.Participants.Add(participant);
        _ctx.SaveChanges();
    }

    public void DeleteParticipant(int participantId, int raceId)
    {
        var race = _ctx.Races.FirstOrDefault(r => r.Id == raceId);
        
        var raceAndParticipant = _ctx.Participants
            .FirstOrDefault(p => p.Id == participantId && p.RaceId == raceId);
        
            // check if there are participants in race
            if (raceAndParticipant is not null && race?.StartRace is null)
            {
                _ctx.Participants.Remove(raceAndParticipant);
                _ctx.SaveChanges();
                return;
            }

            if (raceAndParticipant is null) throw new Exception("Cannot delete participant");
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

        if (race is null) return participantsDto;

        participantsDto.AddRange(race.Participants.Select(participant => new ParticipantDto
        {
            Id = participant.Id,
            UserId = participant.UserId,
            RaceId = participant.RaceId,
            EndTime = participant.EndTime,
            Name = participant.User?.Name ?? "<unknown>",
            Result = participant.EndTime - race.StartRace
        }));

        participantsDto.Sort((participant1, participant2) =>
            TimeSpan.Compare(
                participant1.Result ?? new TimeSpan(0),
                participant2.Result ?? new TimeSpan(0)));


        return participantsDto;
    }
    
}