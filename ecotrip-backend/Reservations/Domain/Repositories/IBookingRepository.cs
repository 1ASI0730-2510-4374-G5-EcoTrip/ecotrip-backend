using ecotrip_backend.Reservations.Domain.Aggregates;

namespace ecotrip_backend.Reservations.Domain.Repositories;
public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task CreateAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(Guid id);
}