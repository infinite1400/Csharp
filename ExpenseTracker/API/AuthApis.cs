

using ExpenseTracker.Models;
using ExpenseTracker.Controllers;
namespace ExpenseTracker.API;

public static class AuthApis
{
    public static void MapAuthApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Auth/Signup", (SignUpRequest request, AuthManager manager) =>
        {
            return AuthController.SignUpController(request, manager);
        });

        app.MapPost("/Auth/SignIn", (SignInRequest request, AuthManager manager) =>
        {
            return AuthController.SignInController(request, manager);
        });

        app.MapGet("/Auth/Users", (AuthManager manager) =>
        {
            return AuthController.ListUsersController(manager);
        });

        app.MapPost("/Auth/editpassword", (EditRequest request, AuthManager manager) =>
        {
            return AuthController.EditPasswordController(request, manager);
        });
    }
}