using ExpenseTracker;
namespace ExpenseTracker.Models;

public class Expense
{
    public Guid id { get; set; }
    public decimal amount { get; set; }
    public string note { get; set; } = string.Empty;
    public char type { get; set; }
    public DateTime date { get; set; } = DateTime.Now;
    public Guid userId { get; set; }
    public Expense(decimal amount, DateTime date, string note, char type, Guid userId)
    {
        this.id = Guid.NewGuid();
        this.amount = amount;
        this.note = note;
        this.type = type;
        this.date = date;
        this.userId = userId;
    }
}

public record AddExpenseRequest(string Note, decimal Amount, char Type, string userId);