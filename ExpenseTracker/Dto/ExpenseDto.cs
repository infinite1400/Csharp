namespace ExpenseTracker.Dto;

using System.Collections.Generic;
using ExpenseTracker.Models;
public class ExpenseDto
{
    public decimal amount { get; set; }
    public string? note { get; set; }
    public string? typeDesc { get; set; }
    public DateTime date { get; set; }

    public static ExpenseDto ToDto(Expense exp)
    {
        return new ExpenseDto
        {
            amount = exp.amount,
            note = exp.note,
            typeDesc = exp.type == 'D' ? "Debit" : "Credit",
            date = exp.date
        };
    }

}

