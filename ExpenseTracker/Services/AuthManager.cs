


using System.Net.Http.Headers;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
        // Paasword Hash
        var hasher = new PasswordHasher<User>();
        var tempUser = new User();
        string hashedPassword = hasher.HashPassword(tempUser, request.Password);
        User user = new User(
            request.Name,
            request.Email,
            hashedPassword
        );
        _users.Users.Add(user);
        await _users.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> ListUsers()
    {
        return await _users.Users.ToListAsync();
    }

    public async Task<User?> EditPasswordAsync(EditRequest request)
    {
        var user = await FindUserAsync(request.Email);
        if (user == null) return null;
        var hasher = new PasswordHasher<User>();
        var tempUser = new User();
        string hashedPassword = hasher.HashPassword(tempUser, request.NewPassword);
        user.Email = request.Email;
        user.Name = request.Name;
        user.Password = hashedPassword;
        await _users.SaveChangesAsync();
        return user;
    }
}