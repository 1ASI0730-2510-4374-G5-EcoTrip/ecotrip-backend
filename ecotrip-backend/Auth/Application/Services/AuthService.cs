using ecotrip_backend.Auth.Application.DTOs;
using ecotrip_backend.Auth.Domain.Aggregates;
using ecotrip_backend.Auth.Domain.Repositories.Interfaces;
using ecotrip_backend.Auth.Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ecotrip_backend.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            throw new InvalidOperationException("Email already registered");

        var hashedPassword = HashPassword(request.Password);
        User user = request.UserType switch
        {
            "Tourist" => TouristUser.Create(request.Email, hashedPassword),
            "Agency" => AgencyUser.Create(request.Email, hashedPassword),
            _ => throw new ArgumentException("Invalid user type")
        };
        
        await _userRepository.CreateAsync(user);

        var token = GenerateToken(user);
        return new AuthResponse(token, user.Email, user.UserType);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {        if (request.UserType != "Tourist" && request.UserType != "Agency")
            throw new ArgumentException("El tipo de usuario debe ser 'Tourist' o 'Agency'");

        var user = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant());
        if (user == null)
            throw new InvalidOperationException("Credenciales inválidas");

        if (user.UserType != request.UserType)
            throw new InvalidOperationException("Este usuario no está registrado como " + request.UserType);

        var hashedPassword = HashPassword(request.Password);if (hashedPassword != user.PasswordHash)
            throw new InvalidOperationException("Credenciales inválidas");

        var token = GenerateToken(user);
        return new AuthResponse(token, user.Email, user.UserType);
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
