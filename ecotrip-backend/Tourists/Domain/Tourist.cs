namespace backendPrueva.Tourists.Domain;

public class Tourist
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Country { get; private set; }

    public Tourist(string fullName, string email, string country)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Country = country;
    }
    
    protected Tourist() { }
}