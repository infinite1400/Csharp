using ExpenseTracker;
using ExpenseTracker.Models;
using ExpenseTracker.Controllers;
using ExpenseTracker.Services;
using ExpenseTracker.Data;
using ExpenseTracker.API;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=expenses.db"));
builder.Services.AddScoped<ExpenseManager>();
builder.Services.AddScoped<AuthManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var expenseManager = services.GetRequiredService<ExpenseManager>();
//     await expenseManager.GenerateDataAsync();
// }


app.UseHttpsRedirection();

// Expense Apis 
app.MapExpenseApis();
app.MapAuthApis();
app.Run();
