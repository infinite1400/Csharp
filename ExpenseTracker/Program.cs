using ExpenseTracker;
using ExpenseTracker.Endpoints;
using ExpenseTracker.Services;
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
    return ExpenseEndpoints.AddExpenseMethod(request, expenseManager);
});

app.MapGet("/ListExpense", () =>
{
    return ExpenseEndpoints.ListExpense(expenseManager);
});

app.MapGet("/expense/{id}", (string id) =>
{
    return ExpenseEndpoints.ExpenseById(id, expenseManager);
});

app.MapPut("/editexpense/{id}", (string id, AddExpenseRequest request) =>
{
    return ExpenseEndpoints.EditExpenseById(id, request, expenseManager);
});

app.MapDelete("/deleteExpense/{id}", (string id) =>
{
    return ExpenseEndpoints.DeleteExpenseById(id, expenseManager);
});

app.Run();

public record AddExpenseRequest(string Note, decimal Amount, char Type);
