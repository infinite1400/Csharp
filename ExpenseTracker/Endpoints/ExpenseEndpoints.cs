using Microsoft.AspNetCore.Http;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Dto;
namespace ExpenseTracker.Endpoints;


public class ExpenseEndpoints
{
    public static async Task<IResult> AddExpenseMethodAsync(AddExpenseRequest request, ExpenseManager manager)
    {
        var (Balance, expense) = await manager.AddExpenseAsync(request.Amount, request.Note, request.Type);
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
        var exp = await manager.FindExpenseByIdAsync(guid);
        if (exp == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        var (Balance, editExpense) = await manager.EditExpenseAsync(guid, request.Amount, request.Note, request.Type);
        return Results.Ok(new { message = "Updated Expense", editExpense = ExpenseDto.ToDto(editExpense), Balance = Balance });
    }

    public static async Task<IResult> DeleteExpenseByIdAsync(string id, ExpenseManager manager)
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
        var (Balance, expense) = await manager.DeleteExpenseAsync(guid);
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
    public static async Task<IResult> ListExpenseByDateAsync(DateTime date, ExpenseManager manager)
    {
        var (balance, DateExpense) = await manager.DateExpenseAsync(date);
        int day = date.Day;
        int month = date.Month;
        int year = date.Year;
        if (!(day >= 1 && day <= 31))
        {
            return Results.BadRequest(new { message = "Enter Valid Month from 1(January) to 12(December)" });
        }
        if (!(month >= 1 && month <= 12))
        {
            return Results.BadRequest(new { message = "Enter Valid Month from 1(January) to 12(December)" });
        }
        if (!(year >= 2000 && year <= 2100))
        {
            return Results.BadRequest(new { message = "Enter Valid Year from 2000 to 2100 !" });
        }
        if (DateExpense == null)
        {
            return Results.NotFound(new { message = $"No Expenses for Month = {month} & Year = {year}" });
        }
        return Results.Ok(new { MonthyBalance = balance, DateExpense = DateExpense });
    }
}