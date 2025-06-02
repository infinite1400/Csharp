namespace ExpenseTracker;

using System;
using System.Text;
public class ExpenseManager
{
    public List<Expense> Expenses = new List<Expense>();
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
        Expenses.Add(exp);
    }
    public Expense? FindExpenseById(Guid id)
    {
        foreach (Expense exp in Expenses)
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
        int Size = Expenses.Count;
        int idx = -1;
        for (int i = 0; i < Size; i++)
        {
            if (Expenses[i].id == id)
            {
                idx = i; break;
            }
        }
        if (type == Expenses[idx].type)
        {
            if (type == 'D')
            {
                // for debit (plus previous debit amount and minus current update
                // amount )
                Balance += Expenses[idx].amount - amount;
            }
            else
            {
                // for credit (minus previous credit amount and add current update
                // amount )
                Balance += -Expenses[idx].amount + amount;
            }
        }
        else
        {
            if (Expenses[idx].type == 'D' && type == 'C')
            {
                Balance += Expenses[idx].amount + amount;
            }
            else
            {
                Balance += -Expenses[idx].amount - amount;
            }
        }
        Expenses[idx].amount = amount;
        Expenses[idx].note = note;
        Expenses[idx].type = type;
        Expenses[idx].date = DateTime.Now;
        return Expenses[idx];
    }
    public Expense DeleteExpense(Guid id)
    {
        int Size = Expenses.Count;
        int idx = -1;
        for (int i = 0; i < Size; i++)
        {
            if (Expenses[i].id == id)
            {
                idx = i; break;
            }
        }
        var exp = Expenses[idx];
        Expenses.Remove(exp);
        return exp;
    }
    public List<Expense> ListExpenses()
    {
        return Expenses;
    }
}