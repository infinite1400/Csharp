using ExpenseTracker;
namespace ExpenseTracker.Models;

public class Expense
{
    public Guid id { get; set; }
    public decimal amount { get; set; }
    public string note { get; set; } = string.Empty;
    public char type { get; set; }
    public DateTime date { get; set; } = DateTime.Now;
    public Expense(decimal amount, DateTime date, string note, char type)
    {
        this.id = Guid.NewGuid();
        this.amount = amount;
        this.note = note;
        this.type = type;
        this.date = date;
    }
}

public record AddExpenseRequest(string Note, decimal Amount, char Type);