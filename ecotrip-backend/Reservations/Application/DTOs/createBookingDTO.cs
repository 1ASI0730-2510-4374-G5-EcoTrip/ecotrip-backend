namespace ecotrip_backend.Reservations.Application.DTOs;
public class CreateBookingDTO
{
    public required string Client { get; set; }
    public DateTime Date { get; set; }
}