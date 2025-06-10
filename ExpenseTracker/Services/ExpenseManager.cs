using System;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Dto;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
namespace ExpenseTracker.Services;

public class ExpenseManager
{

    private readonly AppDbContext _context;

    public ExpenseManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAllExpense()
    {
        var allexp = await _context.Expenses.ToListAsync();
        _context.Expenses.RemoveRange(allexp);
        await _context.SaveChangesAsync();
    }

    public decimal Balance = 0;
    // public async Task GenerateDataAsync()
    // {
    //     var rand = new Random();
    //     for (int i = 0; i < 10; i++)
    //     {
    //         int year = 2025;
    //         int month = rand.Next(1, 7);
    //         int day;
    //         if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
    //         {
    //             day = rand.Next(1, 32);
    //         }
    //         else if (month == 2)
    //         {
    //             day = rand.Next(1, 29);
    //         }
    //         else
    //         {
    //             day = rand.Next(1, 31);
    //         }
    //         int hours = rand.Next(0, 24);
    //         int minutes = rand.Next(0, 60);
    //         int secs = rand.Next(0, 60);
    //         DateTime date = new DateTime(year, month, day, hours, minutes, secs);
    //         string note = $" Expense Number :- {i}";
    //         decimal amount = rand.Next(100, 1000);
    //         char type = i % 2 == 0 ? 'C' : 'D';
    //         Expense exp = new Expense(
    //             amount,
    //             date,
    //             note,
    //             type
    //         );
    //         _context.Expenses.Add(exp);
    //     }
    //     await _context.SaveChangesAsync();
    // }
    public async Task<decimal> CalculateBalance(Guid userId)
    {
        decimal balance = 0;
        List<Expense> Expenses = await _context.Expenses.ToListAsync();
        List<Expense> userExpenses = new List<Expense>();
        foreach (Expense exp in Expenses)
        {
            if (exp.userId == userId)
            {
                userExpenses.Add(exp);
            }
        }
        foreach (Expense expense in userExpenses)
        {
            if (expense.type == 'C')
            {
                balance += expense.amount;
            }
            else
            {
                balance -= expense.amount;
            }
        }
        return balance;
    }
    public decimal CalculateBalance(List<Expense> Expenses)
    {
        decimal balance = 0;
        foreach (Expense expense in Expenses)
        {
            if (expense.type == 'C')
            {
                balance += expense.amount;
            }
            else
            {
                balance -= expense.amount;
            }
        }
        return balance;
    }

    public List<ExpenseDto> ConvertResponse(List<Expense> expenses)
    {
        List<ExpenseDto> expenseDtos = new List<ExpenseDto>();
        foreach (var exp in expenses)
        {
            expenseDtos.Add(ExpenseDto.ToDto(exp));
        }
        return expenseDtos;
    }
    public async Task<(decimal, Expense)> AddExpenseAsync(AddExpenseRequest request, Guid userId)
    {
        Expense exp = new Expense(
            request.Amount,
            DateTime.Now,
            request.Note,
            request.Type,
            userId
        );
        _context.Expenses.Add(exp);
        await _context.SaveChangesAsync();
        decimal balance = await CalculateBalance(userId);
        return (balance, exp);
    }
    public async Task<(decimal, List<Expense>)> ListExpensesAsync(Guid userId)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        List<Expense> userExpenses = new List<Expense>();
        foreach (Expense exp in expenses)
        {
            if (exp.userId == userId)
            {
                userExpenses.Add(exp);
            }
        }
        decimal balance = CalculateBalance(userExpenses);
        return (balance, userExpenses);
    }
    public async Task<Expense?> FindExpenseByIdAsync(Guid id, Guid userId)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        List<Expense> userExpenses = new List<Expense>();
        foreach (Expense exp in expenses)
        {
            if (exp.userId == userId)
            {
                userExpenses.Add(exp);
            }
        }
        foreach (Expense exp in userExpenses)
        {
            if (exp.id == id)
            {
                return exp;
            }
        }
        return null;
    }

    public async Task<(decimal?, Expense?)> EditExpenseAsync(Guid id, decimal amount, string note, char type, Guid userId)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        List<Expense> userExpenses = new List<Expense>();
        foreach (Expense exp in expenses)
        {
            if (exp.userId == userId)
            {
                userExpenses.Add(exp);
            }
        }
        foreach (Expense exp in userExpenses)
        {
            if (exp.id == id)
            {
                exp.amount = amount;
                exp.date = DateTime.Now;
                exp.note = note;
                exp.type = type;
                await _context.SaveChangesAsync();
                decimal Balance = await CalculateBalance(userId);
                return (Balance, exp);
            }
        }
        return (null, null);
    }
    public async Task<(decimal?, Expense?)> DeleteExpenseAsync(Guid id, Guid userId)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return (null, null);
        if (expense.userId == userId)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            decimal Balance = await CalculateBalance(userId);
            return (Balance, expense);
        }
        return (null, null);
    }

    public async Task<(decimal, List<Expense>)> CreditOnlyAsync(Guid userId)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        List<Expense> CreditList = new List<Expense>();
        foreach (Expense exp in expenses)
        {
            if (exp.userId == userId && exp.type == 'C')
            {
                CreditList.Add(exp);
            }
        }
        decimal creditAmount = CalculateBalance(CreditList);
        return (creditAmount, CreditList);
    }

    public async Task<(decimal, List<Expense>)> DebitOnlyAsync(Guid userId)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        List<Expense> DebitList = new List<Expense>();
        foreach (Expense exp in expenses)
        {
            if (exp.userId == userId && exp.type == 'D')
            {
                DebitList.Add(exp);
            }
        }
        decimal debitAmount = CalculateBalance(DebitList);
        return (debitAmount, DebitList);
    }


    public async Task<(decimal, List<ExpenseDto>?)> MonthExpenseAsync(int month, int year)
    {
        List<Expense> MonthlyExpenses = new List<Expense>();
        List<Expense> Expenses = await _context.Expenses.ToListAsync();
        foreach (var exp in Expenses)
        {
            DateTime date = exp.date;
            if (date.Year == year && date.Month == month)
            {
                MonthlyExpenses.Add(exp);
            }
        }
        if (MonthlyExpenses.Count == 0) return (0, null);
        decimal balance = CalculateBalance(MonthlyExpenses);
        List<ExpenseDto> ExpensesDto = ConvertResponse(MonthlyExpenses);
        return (balance, ExpensesDto);
    }
    public async Task<(decimal, List<ExpenseDto>?)> DateExpenseAsync(DateTime date)
    {
        List<Expense> MonthlyExpenses = new List<Expense>();
        List<Expense> Expenses = await _context.Expenses.ToListAsync();
        foreach (var exp in Expenses)
        {
            DateTime Expdate = exp.date;
            if (Expdate.Year == date.Year && Expdate.Month == date.Month && Expdate.Day == date.Day)
            {
                MonthlyExpenses.Add(exp);
            }
        }
        if (MonthlyExpenses.Count == 0) return (0, null);
        decimal balance = CalculateBalance(MonthlyExpenses);
        List<ExpenseDto> ExpensesDto = ConvertResponse(MonthlyExpenses);
        return (balance, ExpensesDto);
    }

    public async Task<(decimal, List<ExpenseDto>?)> ExpenseByRangeAsync(DateTime date1, DateTime date2)
    {
        List<Expense> Expenses = await _context.Expenses.ToListAsync();
        List<Expense> RangeExpense = new List<Expense>();
        foreach (var exp in Expenses)
        {
            DateTime date = exp.date;
            if (date.Date >= date1.Date && date.Date <= date2.Date)
            {
                RangeExpense.Add(exp);
            }
        }
        if (RangeExpense.Count == 0) return (0, null);
        decimal balance = CalculateBalance(RangeExpense);
        List<ExpenseDto> expenseDtos = ConvertResponse(RangeExpense);
        return (balance, expenseDtos);
    }

}