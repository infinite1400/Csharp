
namespace ExpenseTracker.Models;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User(string Name, string Email, string Password)
    {
        this.UserId = Guid.NewGuid();
        this.Name = Name;
        this.Email = Email;
        this.Password = Password;
    }
}

public record SignUpRequest(string Name, string Email, string Password);
public record SignInRequest(string Email, string Password);