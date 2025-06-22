namespace ecotrip_backend.Reservations.Domain.Aggregates;

public class Booking(Guid id, string client, DateTime date)
{
    public Guid Id { get; private set; } = id;
    public string Client { get; private set; } = client;
    public DateTime Date { get; private set; } = date;
}