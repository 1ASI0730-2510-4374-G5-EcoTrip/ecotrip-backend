namespace ecotrip_backend.Tourists.Application.DTOs;

public class UpdateTouristProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
