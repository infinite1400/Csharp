using System;
using System.Text;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services;

public class ExpenseManager
{
    private List<Expense> _allExpenses = new List<Expense>();
    public decimal Balance = 0;

    public void AddExpense(decimal expenseAmount, string expenseNote, char expenseType)
    {
        Expense exp = new Expense(expenseAmount, DateTime.Now, expenseNote, expenseType);
        if (expenseType == 'D')
        {
            Balance -= expenseAmount;
        }
        if (expenseType == 'C')
        {
            Balance += expenseAmount;
        }
        _allExpenses.Add(exp);
    }
    public Expense? FindExpenseById(Guid id)
    {
        foreach (Expense exp in _allExpenses)
        {
            if (id == exp.id)
            {
                return exp;
            }
        }
        return null;
    }

    public Expense EditExpense(Guid id, decimal amount, string note, char type)
    {
        int Size = _allExpenses.Count;
        int idx = -1;
        for (int i = 0; i < Size; i++)
        {
            if (_allExpenses[i].id == id)
            {
                idx = i; break;
            }
        }
        if (type == _allExpenses[idx].type)
        {
            if (type == 'D')
            {
                // for debit (plus previous debit amount and minus current update
                // amount )
                Balance += _allExpenses[idx].amount - amount;
            }
            else
            {
                // for credit (minus previous credit amount and add current update
                // amount )
                Balance += -_allExpenses[idx].amount + amount;
            }
        }
        else
        {
            if (_allExpenses[idx].type == 'D' && type == 'C')
            {
                Balance += _allExpenses[idx].amount + amount;
            }
            else
            {
                Balance += -_allExpenses[idx].amount - amount;
            }
        }
        _allExpenses[idx].amount = amount;
        _allExpenses[idx].note = note;
        _allExpenses[idx].type = type;
        _allExpenses[idx].date = DateTime.Now;
        return _allExpenses[idx];
    }
    public Expense DeleteExpense(Guid id)
    {
        int Size = _allExpenses.Count;
        int idx = -1;
        for (int i = 0; i < Size; i++)
        {
            if (_allExpenses[i].id == id)
            {
                idx = i; break;
            }
        }
        var exp = _allExpenses[idx];
        _allExpenses.Remove(exp);
        return exp;
    }
    public List<Expense> ListExpenses()
    {
        return _allExpenses;
    }

    public (decimal, List<Expense>) CreditOnly()
    {
        decimal bal = 0;
        List<Expense> CreditList = new List<Expense>();
        foreach (Expense exp in _allExpenses)
        {
            if (exp.type == 'C')
            {
                CreditList.Add(exp);
                bal += exp.amount;
            }
        }
        return (bal, CreditList);
    }

    public (decimal, List<Expense>) DebitOnly()
    {
        decimal bal = 0;
        List<Expense> DebitList = new List<Expense>();
        foreach (Expense exp in _allExpenses)
        {
            if (exp.type == 'D')
            {
                DebitList.Add(exp);
                bal -= exp.amount;
            }
        }
        return (bal, DebitList);
    }

}