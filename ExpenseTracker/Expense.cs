using ExpenseTracker;
public class Expense
{
    public Guid id { get; }
    public decimal amount { get; }
    public string note { get; }
    public char type { get; }
    public DateTime date { get; }
    public Expense(decimal amount, DateTime date, string note, char type)
    {
        this.id = Guid.NewGuid();
        this.amount = amount;
        this.note = note;
        this.type = type;
        this.date = date;
    }
}