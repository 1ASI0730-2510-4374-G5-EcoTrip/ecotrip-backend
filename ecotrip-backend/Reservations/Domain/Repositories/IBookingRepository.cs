using ecotrip_backend.Reservations.Domain.Aggregates;

namespace ecotrip_backend.Reservations.Domain.Repositories;
using Reservations.Domain.Aggregates;

public static class BookingRepository
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task CreateAsync(Booking booking);
    }
}