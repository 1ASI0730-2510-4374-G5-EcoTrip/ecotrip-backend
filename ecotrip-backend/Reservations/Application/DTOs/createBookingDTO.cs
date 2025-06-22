namespace ecotrip_backend.Reservations.Application.DTOs;

public class CreateBookingDTo
{
    public required string Client { get; set; }
    public DateTime Date { get; set; }
    
}