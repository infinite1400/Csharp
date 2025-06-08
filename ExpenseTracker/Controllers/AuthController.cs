

using ExpenseTracker.Models;
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
        if (user.Password != request.Password)
        {
            return Results.BadRequest(new { message = "Wrong Password !" });
        }
        return Results.Ok(new { message = "SignIn successful" });
    }
}