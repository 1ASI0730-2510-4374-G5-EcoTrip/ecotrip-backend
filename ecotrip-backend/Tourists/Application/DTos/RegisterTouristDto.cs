namespace ecotrip_backend.Tourists.Application.DTOs;

public class RegisterTouristDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}