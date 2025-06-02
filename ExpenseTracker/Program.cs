using ExpenseTracker;

ExpenseManager expenseManager = new ExpenseManager();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapPost("/Addexpense", (AddExpenseRequest request) =>
{
    expenseManager.AddExpense(request.Amount, request.Note, request.Type);
    Console.WriteLine(request.ToString());
    Console.WriteLine(expenseManager.ListExpenses());
    return Results.Ok(new { message = "Expense added", balance = expenseManager.Balance });
});

app.MapGet("/ListExpense", () =>
{
    return Results.Ok(expenseManager.ListExpenses());
});

app.MapGet("/expense/{id}", (string id) =>
{
    if (Guid.TryParse(id, out var guid) == false)
    {
        return Results.BadRequest(new { message = "Invalid Guid Format" });
    }
    var exp = expenseManager.FindExpenseById(guid);
    if (exp == null)
    {
        return Results.NotFound(new { message = "No Expense by this id" });
    }
    else
    {
        return Results.Ok(exp);
    }
});

app.Run();

public record AddExpenseRequest(string Note, decimal Amount, char Type);
