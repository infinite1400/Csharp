using System;
using System.Text;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
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

    public async Task AddExpenseAsync(decimal expenseAmount, string expenseNote, char expenseType)
    {
        Expense exp = new Expense(
            expenseAmount,
            DateTime.Now,
            expenseNote,
            expenseType
        );
        _context.Expenses.Add(exp);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Expense>> ListExpensesAsync()
    {
        return await _context.Expenses.ToListAsync();
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

    public async Task<Expense?> EditExpenseAsync(Guid id, decimal amount, string note, char type)
    {
        List<Expense> _allExpenses = await _context.Expenses.ToListAsync();
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return null;
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
        return expense;
    }
    public async Task<Expense?> DeleteExpenseAsync(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return null;
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<List<Expense>> CreditOnlyAsync()
    {
        // decimal bal = 0;
        List<Expense> CreditList = new List<Expense>();
        List<Expense> expenses = await _context.Expenses.ToListAsync();
        foreach (Expense exp in expenses)
        {
            if (exp.type == 'C')
            {
                CreditList.Add(exp);
            }
        }
        return CreditList;
    }

    public async Task<List<Expense>> DebitOnlyAsync()
    {
        List<Expense> DebitList = new List<Expense>();
        List<Expense> expenses=await _context.Expenses.ToListAsync();
        foreach (Expense exp in expenses)
        {
            if (exp.type == 'D')
            {
                DebitList.Add(exp);
            }
        }
        return DebitList;
    }

}