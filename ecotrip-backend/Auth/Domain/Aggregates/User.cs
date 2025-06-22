using System.Text.RegularExpressions;

namespace ecotrip_backend.Auth.Domain.Aggregates;

public abstract class User
{
    public string Email { get; protected set; } = null!;
    public string PasswordHash { get; protected set; } = null!;
    public string UserType { get; protected set; } = null!;
     protected User() { }
    
    protected User(string email, string passwordHash, string userType)
    {
        ValidateEmail(email);
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        UserType = userType;
    }

    protected static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format");
    }
}
