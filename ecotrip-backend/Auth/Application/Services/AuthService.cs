using ecotrip_backend.Auth.Application.DTOs;
using ecotrip_backend.Auth.Domain;
using System.Security.Cryptography;
using System.Text;

namespace ecotrip_backend.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly Dictionary<string, User> _users = new();

    public Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (_users.ContainsKey(request.Email.ToLowerInvariant()))
            throw new InvalidOperationException("Email already registered");

        var hashedPassword = HashPassword(request.Password);
        var user = User.Create(request.Email, hashedPassword, request.UserType);
        
        _users[user.Email] = user;

        var token = GenerateToken(user);
        return Task.FromResult(new AuthResponse(token, user.Email, user.UserType));
    }

    public Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = request.Email.ToLowerInvariant();
        if (!_users.TryGetValue(normalizedEmail, out var user))
            throw new InvalidOperationException("Invalid credentials");

        var hashedPassword = HashPassword(request.Password);
        if (hashedPassword != user.PasswordHash)
            throw new InvalidOperationException("Invalid credentials");

        var token = GenerateToken(user);
        return Task.FromResult(new AuthResponse(token, user.Email, user.UserType));
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static string GenerateToken(User user)
    {
        return $"simulated-jwt-token-for-{user.Email}-{user.UserType}";
    }
}
