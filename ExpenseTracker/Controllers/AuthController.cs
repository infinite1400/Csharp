

using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
namespace ExpenseTracker.Controllers;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotenv.net;
using Microsoft.VisualBasic;

public class AuthController
{
    public static string JWTTokenGenerator(User user)
    {
        DotEnv.Load();
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
    public static async Task<IResult> SignUpController(SignUpRequest request, AuthManager manager)
    {
        var user = await manager.SignUpAsync(request);
        if (user == null)
        {
            return Results.BadRequest(new { message = "Email is already taken" });
        }
        else
        {
            return Results.Ok(new { message = "SignUp Success", User = user });
        }
    }

    public static async Task<IResult> SignInController(SignInRequest request, AuthManager manager)
    {

        var user = await manager.FindUserAsync(request.Email);
        if (user == null)
        {
            return Results.NotFound(new { message = "Register first !" });
        }

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);
        if (result == PasswordVerificationResult.Success)
        {
            var AuthToken = JWTTokenGenerator(user);
            return Results.Ok(new { message = "SignIn successful", Token = AuthToken });
        }
        return Results.BadRequest(new { message = "Wrong Password !" });
    }

    public static IResult ListUsersController(AuthManager manager)
    {
        var users = manager.ListUsers();
        return Results.Ok(users);
    }
    public static async Task<IResult> EditPasswordController(EditRequest request, AuthManager manager)
    {
        var user = await manager.FindUserAsync(request.Email);
        if (user == null)
        {
            return Results.NotFound(new { message = "Register first !" });
        }
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
        if (result == PasswordVerificationResult.Failed)
        {
            return Results.Ok(new { message = "Enter Correct Password" });
        }
        var editUser = manager.EditPasswordAsync(request);
        return Results.Ok(new { message = "Password changed successfully" });
    }

}