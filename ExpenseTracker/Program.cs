using ExpenseTracker;
using ExpenseTracker.Models;
using ExpenseTracker.Endpoints;
using ExpenseTracker.Services;
using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
// ExpenseManager expenseManager = new ExpenseManager();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=expenses.db"));
builder.Services.AddScoped<ExpenseManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapPost("/Addexpense", async (AddExpenseRequest request, ExpenseManager manager) =>
{
    await manager.AddExpenseAsync(request.Amount, request.Note, request.Type);
    return Results.Ok(new { message = "Expense Added" });
    // return ExpenseEndpoints.AddExpenseMethod(request, manager);
});

app.MapGet("/ListExpense", async (ExpenseManager manager) =>
{
    List<Expense> expenses = await manager.ListExpensesAsync();
    return Results.Ok(expenses);
    // return ExpenseEndpoints.ListExpense(expenseManager);
});

app.MapGet("/expense/{id}", async (string id,ExpenseManager manager) =>
{
    return await ExpenseEndpoints.ExpenseById(id, manager);
});

// app.MapPut("/editexpense/{id}", (string id, AddExpenseRequest request) =>
// {
//     return ExpenseEndpoints.EditExpenseById(id, request, expenseManager);
// });

// app.MapDelete("/deleteExpense/{id}", (string id) =>
// {
//     return ExpenseEndpoints.DeleteExpenseById(id, expenseManager);
// });

// app.MapGet("/Credits", () =>
// {
//     return ExpenseEndpoints.CreditExpenses(expenseManager);
// });

// app.MapGet("/Debits", () =>
// {
//     return ExpenseEndpoints.DebitExpenses(expenseManager);
// });

app.Run();
