


using System.Net.Http.Headers;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

public class AuthManager
{
    private readonly AppDbContext _users;

    public AuthManager(AppDbContext users)
    {
        _users = users;
    }
    public async Task<User?> FindUserAsync(string Email)
    {
        List<User> Userlist = await _users.Users.ToListAsync();
        foreach (var user in Userlist)
        {
            if (user.Email == Email)
            {
                return user;
            }
        }
        return null;
    }
    public async Task<User?> SignUpAsync(SignUpRequest request)
    {
        var alreadyUser = await FindUserAsync(request.Email);
        if (alreadyUser != null) return null;
        User user = new User(
            request.Name,
            request.Email,
            request.Password
        );
        _users.Users.Add(user);
        await _users.SaveChangesAsync();
        return user;
    }
}