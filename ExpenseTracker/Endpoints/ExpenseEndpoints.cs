using Microsoft.AspNetCore.Http;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Dto;
namespace ExpenseTracker.Endpoints;


public class ExpenseEndpoints
{
    public static async Task<IResult> AddExpenseMethodAsync(AddExpenseRequest request, ExpenseManager manager)
    {
        await manager.AddExpenseAsync(request.Amount, request.Note, request.Type);
        return Results.Ok(new { message = "Expense added" });
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
        return Results.Ok(new { message = "Updated Expense", editExpense, Balance = Balance });
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
        return Results.Ok(new { message = "Deleted Expense", Balance = Balance, DeleteExpense = expense });
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
}