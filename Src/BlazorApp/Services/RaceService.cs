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
    }
}