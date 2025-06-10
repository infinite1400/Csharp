using ExpenseTracker;
using ExpenseTracker.Models;
using ExpenseTracker.Controllers;
using ExpenseTracker.Services;
using ExpenseTracker.Data;
using ExpenseTracker.API;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=expenses.db"));
builder.Services.AddScoped<ExpenseManager>();
builder.Services.AddScoped<AuthManager>();
DotEnv.Load();
string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
{
    throw new Exception("JWT_SECRET_KEY is missing or too short (must be at least 16 characters).");
}
var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();
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
//     // await expenseManager.GenerateDataAsync();
//     // await expenseManager.DeleteAllExpense();
//     Console.WriteLine("Task Done !");
// }
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Expense Apis 
app.MapExpenseApis();
app.MapAuthApis();
app.Run();

