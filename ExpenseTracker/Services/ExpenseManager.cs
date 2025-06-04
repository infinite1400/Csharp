using System;
using System.Text;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
namespace ExpenseTracker.Services;

public class ExpenseManager
{

    private readonly AppDbContext _context;

    public ExpenseManager(AppDbContext context)
    {
        _context = context;
    }

    private List<Expense> _allExpenses = new List<Expense>();
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

    // public Expense EditExpense(Guid id, decimal amount, string note, char type)
    // {
    //     int Size = _allExpenses.Count;
    //     int idx = -1;
    //     for (int i = 0; i < Size; i++)
    //     {
    //         if (_allExpenses[i].id == id)
    //         {
    //             idx = i; break;
    //         }
    //     }
    //     if (type == _allExpenses[idx].type)
    //     {
    //         if (type == 'D')
    //         {
    //             // for debit (plus previous debit amount and minus current update
    //             // amount )
    //             Balance += _allExpenses[idx].amount - amount;
    //         }
    //         else
    //         {
    //             // for credit (minus previous credit amount and add current update
    //             // amount )
    //             Balance += -_allExpenses[idx].amount + amount;
    //         }
    //     }
    //     else
    //     {
    //         if (_allExpenses[idx].type == 'D' && type == 'C')
    //         {
    //             Balance += _allExpenses[idx].amount + amount;
    //         }
    //         else
    //         {
    //             Balance += -_allExpenses[idx].amount - amount;
    //         }
    //     }
    //     _allExpenses[idx].amount = amount;
    //     _allExpenses[idx].note = note;
    //     _allExpenses[idx].type = type;
    //     _allExpenses[idx].date = DateTime.Now;
    //     return _allExpenses[idx];
    // }
    // public Expense DeleteExpense(Guid id)
    // {
    //     int Size = _allExpenses.Count;
    //     int idx = -1;
    //     for (int i = 0; i < Size; i++)
    //     {
    //         if (_allExpenses[i].id == id)
    //         {
    //             idx = i; break;
    //         }
    //     }
    //     var exp = _allExpenses[idx];
    //     _allExpenses.Remove(exp);
    //     return exp;
    // }

    // public (decimal, List<Expense>) CreditOnly()
    // {
    //     decimal bal = 0;
    //     List<Expense> CreditList = new List<Expense>();
    //     foreach (Expense exp in _allExpenses)
    //     {
    //         if (exp.type == 'C')
    //         {
    //             CreditList.Add(exp);
    //             bal += exp.amount;
    //         }
    //     }
    //     return (bal, CreditList);
    // }

    // public (decimal, List<Expense>) DebitOnly()
    // {
    //     decimal bal = 0;
    //     List<Expense> DebitList = new List<Expense>();
    //     foreach (Expense exp in _allExpenses)
    //     {
    //         if (exp.type == 'D')
    //         {
    //             DebitList.Add(exp);
    //             bal -= exp.amount;
    //         }
    //     }
    //     return (bal, DebitList);
    // }

}