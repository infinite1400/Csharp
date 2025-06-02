using ExpenseTracker;
public class Expense
{
    public Guid id { get; }
    public decimal amount { get; set; }
    public string note { get; set; }
    public char type { get; set; }
    public DateTime date { get; set; }
    public Expense(decimal amount, DateTime date, string note, char type)
    {
        this.id = Guid.NewGuid();
        this.amount = amount;
        this.note = note;
        this.type = type;
        this.date = date;
    }
}