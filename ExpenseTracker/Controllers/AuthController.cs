

using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
namespace ExpenseTracker.Controllers;

public class AuthController
{
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
            return Results.Ok(new { message = "SignIn successful" });
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