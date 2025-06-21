namespace ecotrip_backend.Auth.Domain.Aggregates;

public class TouristUser : User
{
    public TouristUser(string email, string passwordHash) 
        : base(email, passwordHash, "Tourist")
    {
    }

    public static TouristUser Create(string email, string passwordHash)
    {
        return new TouristUser(email, passwordHash);
    }
}
