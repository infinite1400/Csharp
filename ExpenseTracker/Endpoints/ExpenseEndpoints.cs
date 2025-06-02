using Microsoft.AspNetCore.Http;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
namespace ExpenseTracker.Endpoints;


public class ExpenseEndpoints
{
    public static IResult AddExpenseMethod(AddExpenseRequest request, ExpenseManager manager)
    {
        manager.AddExpense(request.Amount, request.Note, request.Type);
        Console.WriteLine(request.ToString());
        Console.WriteLine(manager.ListExpenses());
        return Results.Ok(new { message = "Expense added", balance = manager.Balance });
    }

    public static IResult ListExpense(ExpenseManager manager)
    {
        return Results.Ok(new { Balance = manager.Balance, Expenses = manager.ListExpenses() });
    }

    public static IResult ExpenseById(string id, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var exp = manager.FindExpenseById(guid);
        if (exp == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        else
        {
            return Results.Ok(exp);
        }
    }

    public static IResult EditExpenseById(string id, AddExpenseRequest request, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var exp = manager.FindExpenseById(guid);
        if (exp == null)
        {
            return Results.NotFound(new { message = "No Expense by this id" });
        }
        var editExpense = manager.EditExpense(guid, request.Amount, request.Note, request.Type);
        return Results.Ok(new { message = "Updated Expense", editExpense, balance = manager.Balance });
    }

    public static IResult DeleteExpenseById(string id, ExpenseManager manager)
    {
        if (Guid.TryParse(id, out var guid) == false)
        {
            return Results.BadRequest(new { message = "Invalid Guid Format" });
        }
        var exp = manager.FindExpenseById(guid);
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
        return Results.Ok(new { message = "Deleted Expense", DeleteExpense = manager.DeleteExpense(guid), balance = manager.Balance });
    }

    public static IResult CreditExpenses(ExpenseManager manager)
    {
        var (balance, creditList) = manager.CreditOnly();
        return Results.Ok(new { balance = balance, Credits = creditList });
    }

    public static IResult DebitExpenses(ExpenseManager manager)
    {
        var (balance, debitList) = manager.DebitOnly();
        return Results.Ok(new { balance = balance, Debits = debitList });
    }
}