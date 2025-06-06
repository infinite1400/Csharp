using ExpenseTracker;
using ExpenseTracker.Models;
using ExpenseTracker.Endpoints;
using ExpenseTracker.Services;
using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var expenseManager = services.GetRequiredService<ExpenseManager>();
    await expenseManager.GenerateDataAsync();
}


app.UseHttpsRedirection();
app.MapPost("/Addexpense", async (AddExpenseRequest request, ExpenseManager manager) =>
{
    return await ExpenseEndpoints.AddExpenseMethodAsync(request, manager);
    // return Results.Ok(new { message = "Expense Added" });
});

app.MapGet("/ListExpense", async (ExpenseManager manager) =>
{
    return await ExpenseEndpoints.ListExpenseMethodAsync(manager);
});

app.MapGet("/expense/{id}", async (string id, ExpenseManager manager) =>
{
    return await ExpenseEndpoints.ExpenseById(id, manager);
});

app.MapPut("/editexpense/{id}", (string id, AddExpenseRequest request, ExpenseManager manager) =>
{
    return ExpenseEndpoints.EditExpenseByIdAsync(id, request, manager);
});

app.MapDelete("/deleteExpense/{id}", (string id, ExpenseManager manager) =>
{
    return ExpenseEndpoints.DeleteExpenseByIdAsync(id, manager);
});

app.MapGet("/Credits", (ExpenseManager manager) =>
{
    return ExpenseEndpoints.CreditExpensesAsync(manager);
});

app.MapGet("/Debits", (ExpenseManager manager) =>
{
    return ExpenseEndpoints.DebitExpensesAsync(manager);
});

app.MapGet("/MonthlyExpense/month={month}/year={year}", (int month, int year, ExpenseManager manager) =>
{
    return ExpenseEndpoints.ListExpenseByMonthAsync(month, year, manager);
});
app.MapGet("/DateExpense/day={day}/month={month}/year={year}", (int day, int month, int year, ExpenseManager manager) =>
{
    DateTime date = new(year, month, day);
    return ExpenseEndpoints.ListExpenseByDateAsync(date, manager);
});

app.Run();
