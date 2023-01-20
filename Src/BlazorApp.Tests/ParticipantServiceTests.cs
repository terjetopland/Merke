using BlazorApp.Data;
using BlazorApp.Dtos;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Tests;

public class ParticipantServiceTests
{
    // Add unit test for ParticipantService
    [Fact]
    public async Task GetParticipants_ShouldReturnParticpantsForARace()
    {
        // temporary in memory database
        var dbOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("TestDb");
        
        // arrange
        const int raceIdTest = 2;
        var raceEndDateTest = new DateTime(2021, 12, 31, 10,59,59);
        var userEndDateTest = new DateTime(2021, 12, 31, 11, 23, 12);
        
        var particpantDtos = new List<ParticipantDto>
        {
            new()
            {
                Id = 1,
                RaceId = raceIdTest,
                EndTime = userEndDateTest
            },
            new()
            {
                Id = 2,
                RaceId = raceIdTest,
                EndTime = userEndDateTest
            },
            new()
            {
                Id = 3,
                RaceId = raceIdTest,
                EndTime = userEndDateTest
            }
        };
        await using (var db = new AppDbContext(dbOptionsBuilder.Options))
        {
            // add some test data to the in memory database
            await db.Set<Race>().AddAsync(new Race{ Id = raceIdTest, EndRace = raceEndDateTest});
            await db.Set<Participant>().AddRangeAsync(
                particpantDtos.Select(x => new Participant {Id = x.Id, RaceId = x.RaceId, EndTime = x.EndTime})
            );
            await db.SaveChangesAsync();
        }
        
        await using (var db = new AppDbContext(dbOptionsBuilder.Options))
        {
            // create the service
            var service = new ParticipantService(db);

            // act
            var result = service.GetParticipants(raceIdTest);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(particpantDtos.Select(x => new { x.Id, x.RaceId, x.EndTime}), result.Select(x => new { x.Id, x.RaceId, x.EndTime }));
        }
    }
}