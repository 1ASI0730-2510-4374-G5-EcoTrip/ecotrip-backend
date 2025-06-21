namespace ecotrip_backend.Tourists.Domain;

public class Tourist
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string? Bio { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Tourist(string fullName, string email, string country)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Country = country;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    protected Tourist() { }
    
    public void UpdateProfile(string fullName, string country, string? bio = null, string? profilePictureUrl = null)
    {
        FullName = fullName;
        Country = country;
        
        if (bio != null)
            Bio = bio;
            
        if (profilePictureUrl != null)
            ProfilePictureUrl = profilePictureUrl;
            
        UpdatedAt = DateTime.UtcNow;
    }
}