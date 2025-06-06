using System;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Dto;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
namespace ExpenseTracker.Services;

public class ExpenseManager
{

    private readonly AppDbContext _context;

    public ExpenseManager(AppDbContext context)
    {
        _context = context;
    }

    public decimal Balance = 0;
    public async Task GenerateDataAsync()
    {
        var rand = new Random();
        for (int i = 0; i < 100; i++)
        {
            int year = 2025;
            int month = rand.Next(1, 7);
            int day;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = rand.Next(1, 32);
            }
            else if (month == 2)
            {
                day = rand.Next(1, 29);
            }
            else
            {
                day = rand.Next(1, 31);
            }
            int hours = rand.Next(0, 24);
            int minutes = rand.Next(0, 60);
            int secs = rand.Next(0, 60);
            DateTime date = new DateTime(year, month, day, hours, minutes, secs);
            string note = $" Expense Number :- {i}";
            decimal amount = rand.Next(100, 1000);
            char type = i % 2 == 0 ? 'C' : 'D';
            Expense exp = new Expense(
                amount,
                date,
                note,
                type
            );
            _context.Expenses.Add(exp);
        }
        await _context.SaveChangesAsync();
    }
    public async Task<decimal> CalculateBalance()
    {
        decimal balance = 0;
        List<Expense> Expenses = await _context.Expenses.ToListAsync();
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
    public async Task<(decimal, Expense)> AddExpenseAsync(decimal expenseAmount, string expenseNote, char expenseType)
    {
        Expense exp = new Expense(
            expenseAmount,
            DateTime.Now,
            expenseNote,
            expenseType
        );
        _context.Expenses.Add(exp);
        await _context.SaveChangesAsync();
        decimal balance = await CalculateBalance();
        return (balance, exp);
    }
    public async Task<(decimal, List<Expense>)> ListExpensesAsync()
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        decimal balance = await CalculateBalance();
        return (balance, expenses);
    }
    public async Task<Expense?> FindExpenseByIdAsync(Guid id)
    {
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        foreach (Expense exp in expenses)
        {
            if (id == exp.id)
            {
                return exp;
            }
        }
        return null;
    }

    public async Task<(decimal?, Expense?)> EditExpenseAsync(Guid id, decimal amount, string note, char type)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return (null, null);
        decimal Balance = await CalculateBalance();
        if (type == expense.type)
        {
            if (type == 'D')
            {
                // for debit (plus previous debit amount and minus current update
                // amount )
                Balance += expense.amount - amount;
            }
            else
            {
                // for credit (minus previous credit amount and add current update
                // amount )
                Balance += -expense.amount + amount;
            }
        }
        else
        {
            if (expense.type == 'D' && type == 'C')
            {
                Balance += expense.amount + amount;
            }
            else
            {
                Balance += -expense.amount - amount;
            }
        }
        expense.amount = amount;
        expense.note = note;
        expense.type = type;
        expense.date = DateTime.Now;
        await _context.SaveChangesAsync();
        return (Balance, expense);
    }
    public async Task<(decimal?, Expense?)> DeleteExpenseAsync(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return (null, null);
        _context.Expenses.Remove(expense);
        decimal Balance = await CalculateBalance();
        if (expense.type == 'C')
        {
            Balance -= expense.amount;
        }
        else
        {
            Balance += expense.amount;
        }
        await _context.SaveChangesAsync();
        return (Balance, expense);
    }

    public async Task<(decimal, List<Expense>)> CreditOnlyAsync()
    {
        List<Expense> CreditList = new List<Expense>();
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        foreach (Expense exp in expenses)
        {
            if (exp.type == 'C')
            {
                CreditList.Add(exp);
            }
        }
        decimal creditAmount = CalculateBalance(CreditList);
        return (creditAmount, CreditList);
    }

    public async Task<(decimal, List<Expense>)> DebitOnlyAsync()
    {
        List<Expense> DebitList = new List<Expense>();
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        foreach (Expense exp in expenses)
        {
            if (exp.type == 'D')
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

}