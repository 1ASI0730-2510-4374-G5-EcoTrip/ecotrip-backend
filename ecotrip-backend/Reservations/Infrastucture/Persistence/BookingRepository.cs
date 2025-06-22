using ecotrip_backend.Reservations.Domain.Aggregates;
using ecotrip_backend.Reservations.Domain.Repositories;

namespace ecotrip_backend.Reservations.Infrastructure.Persistence;

public class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();

    public Task<Booking?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
    }

    public Task<IEnumerable<Booking>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Booking>>(_bookings.ToList());
    }

    public Task CreateAsync(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));
            
        _bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        var existingBooking = _bookings.FirstOrDefault(b => b.Id == booking.Id);
        if (existingBooking != null)
        {
            _bookings.Remove(existingBooking);
            _bookings.Add(booking);
        }
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
        {
            _bookings.Remove(booking);
        }
        
        return Task.CompletedTask;
    }
}