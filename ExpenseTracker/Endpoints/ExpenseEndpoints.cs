using Microsoft.AspNetCore.Http;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Dto;
namespace ExpenseTracker.Endpoints;


public class ExpenseEndpoints
{
    // public static IResult AddExpenseMethod(AddExpenseRequest request, ExpenseManager manager)
    // {
    //     manager.AddExpense(request.Amount, request.Note, request.Type);
    //     Console.WriteLine(request.ToString());
    //     Console.WriteLine(manager.ListExpenses());
    //     return Results.Ok(new { message = "Expense added", balance = manager.Balance });
    // }

    // public static IResult ListExpense(ExpenseManager manager)
    // {
    //     return Results.Ok(new { Balance = manager.Balance, Expenses = manager.ListExpenses() });
    // }

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
        var editExpense = manager.EditExpenseAsync(guid, request.Amount, request.Note, request.Type);
        return Results.Ok(new { message = "Updated Expense", editExpense, balance = manager.Balance });
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
        if (exp.type == 'C')
        {
            manager.Balance -= exp.amount;
        }
        else
        {
            manager.Balance += exp.amount;
        }
        return Results.Ok(new { message = "Deleted Expense", DeleteExpense = manager.DeleteExpenseAsync(guid), balance = manager.Balance });
    }

    public static async Task<IResult> CreditExpensesAsync(ExpenseManager manager)
    {
        var creditList = await manager.CreditOnlyAsync();
        List<ExpenseDto> creditDto = new List<ExpenseDto>();
        foreach (var exp in creditList)
        {
            creditDto.Add(ExpenseDto.ToDto(exp));
        }
        return Results.Ok(new {Credits = creditDto });
    }

    public static async Task<IResult> DebitExpensesAsync(ExpenseManager manager)
    {
        var debitList = await manager.DebitOnlyAsync();
        List<ExpenseDto> debitDto = new List<ExpenseDto>();
        foreach (var exp in debitList)
        {
            debitDto.Add(ExpenseDto.ToDto(exp));
        }
        return Results.Ok(new { Debits = debitDto });
    }
}