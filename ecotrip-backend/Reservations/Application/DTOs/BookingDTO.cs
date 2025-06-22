namespace ecotrip_backend.Reservations.Application.DTOs;
public class BookingDTO
{  
      public Guid Id { get; set; }
    public required string Client { get; set; }
    public DateTime Date { get; set; }
}