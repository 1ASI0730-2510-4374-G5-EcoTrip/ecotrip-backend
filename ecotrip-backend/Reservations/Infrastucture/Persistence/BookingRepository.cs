using ecotrip_backend.Reservations.Domain.Aggregates;

namespace ecotrip_backend.Reservations.Infrastructure.Persistence;

using Reservations.Domain.Aggregates;
using Reservations.Domain.Repositories;

public class BookingRepository : Domain.Repositories.BookingRepository.IBookingRepository
{
    private readonly List<Booking> _bookings = new();

    public Task<Booking?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
    }

    public Task CreateAsync(Booking booking)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }
}