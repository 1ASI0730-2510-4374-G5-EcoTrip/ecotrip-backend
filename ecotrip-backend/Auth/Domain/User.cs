using System.Text.RegularExpressions;

namespace ecotrip_backend.Auth.Domain;

public class User
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string UserType { get; private set; }

    private User(string email, string passwordHash, string userType)
    {
        Email = email;
        PasswordHash = passwordHash;
        UserType = userType;
    }

    public static User Create(string email, string passwordHash, string userType)
    {
        ValidateEmail(email);
        ValidateUserType(userType);

        return new User(
            email: email.ToLowerInvariant(),
            passwordHash: passwordHash,
            userType: userType
        );
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format");
    }

    private static void ValidateUserType(string userType)
    {
        if (string.IsNullOrWhiteSpace(userType))
            throw new ArgumentException("User type cannot be empty");

        if (userType != "Tourist" && userType != "Agency")
            throw new ArgumentException("User type must be either 'Tourist' or 'Agency'");
    }
}
