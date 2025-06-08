
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
    public User()
    {
        UserId = Guid.Empty;
        Name = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }
}

public record SignUpRequest(string Name, string Email, string Password);
public record SignInRequest(string Email, string Password);
public record EditRequest(string Name, string Email, string OldPassword,string NewPassword);
