namespace ecotrip_backend.Auth.Application.DTOs
{
    public sealed record LoginRequest(string Email, string Password, string UserType);

    public sealed record RegisterRequest(string Email, string Password, string UserType);

    public sealed record AuthResponse(string Token, string Email, string UserType);
}
    