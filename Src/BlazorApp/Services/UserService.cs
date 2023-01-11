using BlazorApp.Data;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IUserService
{
    public List<AppUser> GetUsers();
}

public class UserService :IUserService
{
    private readonly AppDbContext _ctx;

    public UserService(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public List<AppUser> GetUsers()
    {
        var users = _ctx.Users.ToList();
        return users;



        }
}