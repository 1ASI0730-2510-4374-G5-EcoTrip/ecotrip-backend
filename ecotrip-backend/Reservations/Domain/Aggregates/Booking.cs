namespace ecotrip_backend.Reservations.Domain.Aggregates;
public class Booking
{
    public Guid Id { get; private set; }
    public string Client { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private Booking() 
    {
        Client = string.Empty;
    }
    public Booking(Guid id, string client, DateTime date)
    {
        Id = id;
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Date = date;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Update(string client, DateTime date)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Date = date;
        UpdatedAt = DateTime.UtcNow;
    }
}