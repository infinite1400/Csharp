
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Controllers;
namespace ExpenseTracker.API;

public static class ExpenseApis
{
    public static void MapExpenseApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Addexpense", async (AddExpenseRequest request, ExpenseManager manager) =>
        {
            return await ExpenseController.AddExpenseMethodAsync(request, manager);
            // return Results.Ok(new { message = "Expense Added" });
        });

        app.MapGet("/ListExpense", async (ExpenseManager manager) =>
        {
            return await ExpenseController.ListExpenseMethodAsync(manager);
        });

        app.MapGet("/expense/{id}", async (string id, ExpenseManager manager) =>
        {
            return await ExpenseController.ExpenseById(id, manager);
        });

        app.MapPut("/editexpense/{id}", (string id, AddExpenseRequest request, ExpenseManager manager) =>
        {
            return ExpenseController.EditExpenseByIdAsync(id, request, manager);
        });

        app.MapDelete("/deleteExpense/{id}", (string id, ExpenseManager manager) =>
        {
            return ExpenseController.DeleteExpenseByIdAsync(id, manager);
        });

        app.MapGet("/Credits", (ExpenseManager manager) =>
        {
            return ExpenseController.CreditExpensesAsync(manager);
        });

        app.MapGet("/Debits", (ExpenseManager manager) =>
        {
            return ExpenseController.DebitExpensesAsync(manager);
        });

        app.MapGet("/MonthlyExpense/month={month}/year={year}", (int month, int year, ExpenseManager manager) =>
        {
            return ExpenseController.ListExpenseByMonthAsync(month, year, manager);
        });
        app.MapGet("/DateExpense/day={day}/month={month}/year={year}", (int day, int month, int year, ExpenseManager manager) =>
        {
            DateTime date = new(year, month, day);
            return ExpenseController.ListExpenseByDateAsync(date, manager);
        });
    }
}