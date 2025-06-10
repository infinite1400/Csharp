
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
    public static async Task<IResult> AddExpenseMethod(HttpContext http, AddExpenseRequest request, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.AddExpenseMethodAsync(request, manager, userId);
    }

    [Authorize]
    public static async Task<IResult> ListExpensesMethod(HttpContext http, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.ListExpenseMethodAsync(manager, userId);
    }
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
    [Authorize]
    public static async Task<IResult> MonthlyExpensesMethod(HttpContext http, int month, int year, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.ListExpenseByMonthAsync(month, year, manager, userId);
    }
    [Authorize]
    public static async Task<IResult> DateExpensesMethod(HttpContext http, string date, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.ListExpenseByDateAsync(date, manager, userId);
    }
    [Authorize]
    public static async Task<IResult> RangeDateExpensesMethod(HttpContext http, string date1, string date2, ExpenseManager manager)
    {
        var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        Guid userId = Guid.Parse(userIdClaim.Value);
        return await ExpenseController.ListExpenseByRangeAsync(date1, date2, manager, userId);
    }
    public static void MapExpenseApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Addexpense", AddExpenseMethod);

        app.MapGet("/ListExpense", ListExpensesMethod);

        app.MapGet("/expense/{id}", ExpenseByIdMethod);

        app.MapPut("/editexpense/{id}", EditExpenseMethod);

        app.MapDelete("/deleteExpense/{id}", DeleteExpenseMethod);

        app.MapGet("/Credits", CreditListMethod);

        app.MapGet("/Debits", DebitListMethod);

        app.MapGet("/MonthlyExpense/month={month}/year={year}", MonthlyExpensesMethod);

        app.MapGet("/DateExpense/Date={date}", DateExpensesMethod);

        app.MapGet("/RangeExpenses/Date1={date1}/Date2={date2}", RangeDateExpensesMethod);
    }
}