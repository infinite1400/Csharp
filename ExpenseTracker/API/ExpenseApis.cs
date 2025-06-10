
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace ExpenseTracker.API;

public static class ExpenseApis
{
    [Authorize]
    public static async Task<IResult> DeleteExpenseMethod(HttpContext http, string id, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.DeleteExpenseByIdAsync(id, manager, userId);
    }

    [Authorize]
    public static async Task<IResult> EditExpenseMethod(HttpContext http, string id, AddExpenseRequest request, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.EditExpenseByIdAsync(id, request, manager, userId);
    }
    [Authorize]
    public static async Task<IResult> ExpenseByIdMethod(HttpContext http, string id, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.ExpenseById(id, manager, userId);
    }
    [Authorize]
    public static async Task<IResult> DebitListMethod(HttpContext http, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userid = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.DebitExpensesAsync(manager, userid);
    }

    [Authorize]
    public static async Task<IResult> CreditListMethod(HttpContext http, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userid = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.CreditExpensesAsync(manager, userid);
    }
    public static void MapExpenseApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Addexpense", [Authorize] async (HttpContext http, AddExpenseRequest request, ExpenseManager manager) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Results.Unauthorized();
            }
            Guid userId = Guid.Parse(userIdClaim.Value);
            return await ExpenseController.AddExpenseMethodAsync(request, manager, userId);
        });

        app.MapGet("/ListExpense", [Authorize] async (HttpContext http, ExpenseManager manager) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Results.Unauthorized();
            }
            Guid userId = Guid.Parse(userIdClaim.Value);
            return await ExpenseController.ListExpenseMethodAsync(manager, userId);
        });

        app.MapGet("/expense/{id}", ExpenseByIdMethod);

        app.MapPut("/editexpense/{id}", EditExpenseMethod);

        app.MapDelete("/deleteExpense/{id}", DeleteExpenseMethod);

        app.MapGet("/Credits", CreditListMethod);

        app.MapGet("/Debits", DebitListMethod);

        app.MapGet("/MonthlyExpense/month={month}/year={year}", (int month, int year, ExpenseManager manager) =>
        {
            return ExpenseController.ListExpenseByMonthAsync(month, year, manager);
        });
        app.MapGet("/DateExpense/Date={date}", (string date, ExpenseManager manager) =>
        {
            return ExpenseController.ListExpenseByDateAsync(date, manager);
        });

        app.MapGet("/RangeExpenses/Date1={date1}/Date2={date2}", (string date1, string date2, ExpenseManager manager) =>
        {
            return ExpenseController.ListExpenseByRangeAsync(date1, date2, manager);
        });
    }
}