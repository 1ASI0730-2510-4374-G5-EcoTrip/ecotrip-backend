using ecotrip_backend.Auth.Application.DTOs;

namespace ecotrip_backend.Auth.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
