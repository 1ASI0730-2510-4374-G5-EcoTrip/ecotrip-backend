namespace ecotrip_backend.Auth.Domain.Aggregates;

public class AgencyUser : User
{
    public AgencyUser(string email, string passwordHash) 
        : base(email, passwordHash, "Agency")
    {
    }

    public static AgencyUser Create(string email, string passwordHash)
    {
        return new AgencyUser(email, passwordHash);
    }
}
