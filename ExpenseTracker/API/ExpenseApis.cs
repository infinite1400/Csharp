
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