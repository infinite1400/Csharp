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
    public List<Expense> ListExpenses()
    {
        return Expenses;
    }
}