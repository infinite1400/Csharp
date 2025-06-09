using Microsoft.AspNetCore.Http;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Dto;
namespace ExpenseTracker.Controllers;


public class ExpenseController
{
    public static async Task<IResult> AddExpenseMethodAsync(AddExpenseRequest request, ExpenseManager manager, Guid userId)
    {
        var (Balance, expense) = await manager.AddExpenseAsync(request, userId);
        return Results.Ok(new { message = "Expense added", Balance = Balance, Expense = expense });
    }

    public static async Task<IResult> ListExpenseMethodAsync(ExpenseManager manager)
    {
        var (balance, expenses) = await manager.ListExpensesAsync();
        return Results.Ok(new { balance = balance, Expenses = expenses });
    }

    public static async Task<IResult> ExpenseById(string id, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var exp = await manager.FindExpenseByIdAsync(guid);
        if (exp == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        else
        {
            return Results.Ok(ExpenseDto.ToDto(exp));
        }
    }

    public static async Task<IResult> EditExpenseByIdAsync(string id, AddExpenseRequest request, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var (Balance, editExpense) = await manager.EditExpenseAsync(guid, request.Amount, request.Note, request.Type);
        if (editExpense == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        return Results.Ok(new { message = "Updated Expense", editExpense = ExpenseDto.ToDto(editExpense), Balance = Balance });
    }

    public static async Task<IResult> DeleteExpenseByIdAsync(string id, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var (Balance, expense) = await manager.DeleteExpenseAsync(guid);
        if (expense == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        return Results.Ok(new { message = "Deleted Expense", Balance = Balance, DeleteExpense = ExpenseDto.ToDto(expense) });
    }

    public static async Task<IResult> CreditExpensesAsync(ExpenseManager manager)
    {
        var (credit, creditList) = await manager.CreditOnlyAsync();
        List<ExpenseDto> creditDto = new List<ExpenseDto>();
        foreach (var exp in creditList)
        {
            creditDto.Add(ExpenseDto.ToDto(exp));
        }
        return Results.Ok(new { CreditAmount = credit, Credits = creditDto });
    }

    public static async Task<IResult> DebitExpensesAsync(ExpenseManager manager)
    {
        var (debit, debitList) = await manager.DebitOnlyAsync();
        List<ExpenseDto> debitDto = new List<ExpenseDto>();
        foreach (var exp in debitList)
        {
            debitDto.Add(ExpenseDto.ToDto(exp));
        }
        return Results.Ok(new { deditAmount = debit, Debits = debitDto });
    }

    public static async Task<IResult> ListExpenseByMonthAsync(int month, int year, ExpenseManager manager)
    {
        var (balance, MonthlyExpense) = await manager.MonthExpenseAsync(month, year);
        if (!(month >= 1 && month <= 12))
        {
            return Results.BadRequest(new { message = "Enter Valid Month from 1(January) to 12(December)" });
        }
        if (!(year >= 2000 && year <= 2100))
        {
            return Results.BadRequest(new { message = "Enter Valid Year from 2000 to 2100 !" });
        }
        if (MonthlyExpense == null)
        {
            return Results.NotFound(new { message = $"No Expenses for Month = {month} & Year = {year}" });
        }
        return Results.Ok(new { MonthyBalance = balance, MonthlyExpense = MonthlyExpense });
    }
    public static async Task<IResult> ListExpenseByDateAsync(string date, ExpenseManager manager)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return Results.BadRequest(new { message = "Enter date correctly in format YYYY-MM-DD" });
        }
        var (balance, DateExpense) = await manager.DateExpenseAsync(parsedDate);
        if (DateExpense == null)
        {
            return Results.NotFound(new { message = $"No Expenses for Month = {parsedDate.Month} & Year = {parsedDate.Year}" });
        }
        return Results.Ok(new { MonthyBalance = balance, DateExpense = DateExpense });
    }

    public static async Task<IResult> ListExpenseByRangeAsync(string date1, string date2, ExpenseManager manager)
    {
        // validating date1
        if (!DateTime.TryParse(date1, out var parsedDate1))
        {
            return Results.BadRequest(new { message = "Enter date1 correctly in format YYYY-MM-DD" });
        }
        // validating date 2
        if (!DateTime.TryParse(date2, out var parsedDate2))
        {
            return Results.BadRequest(new { message = "Enter date2 correctly in format YYYY-MM-DD" });
        }
        var (balance, RangeExpenses) = await manager.ExpenseByRangeAsync(parsedDate1, parsedDate2);
        if (RangeExpenses == null)
        {
            return Results.NotFound(new { message = "No expenses in this Date Range" });
        }
        return Results.Ok(new { RangeBalance = balance, RangeExpenses = RangeExpenses });
    }
}