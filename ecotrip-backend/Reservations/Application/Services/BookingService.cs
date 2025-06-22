using ecotrip_backend.Reservations.Application.DTOs;
using ecotrip_backend.Reservations.Domain.Aggregates;
using ecotrip_backend.Reservations.Domain.Repositories;

namespace ecotrip_backend.Reservations.Application.Services;

public class BookingService
{
    private readonly IBookingRepository _repository;
    public BookingService(IBookingRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<Guid> CreateBookingAsync(CreateBookingDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var booking = new Booking(Guid.NewGuid(), dto.Client, dto.Date);
        await _repository.CreateAsync(booking);
        return booking.Id;
    }
    public async Task<BookingDTO> GetBookingAsync(Guid id)
    {
        var booking = await _repository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException($"Booking with ID {id} not found");

        return new BookingDTO
        {
            Id = booking.Id,
            Client = booking.Client,
            Date = booking.Date
        };
    }
    public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
    {
        var bookings = await _repository.GetAllAsync();
        return bookings.Select(b => new BookingDTO
        {
            Id = b.Id,
            Client = b.Client,
            Date = b.Date
        });
    }

    public async Task UpdateBookingAsync(Guid id, CreateBookingDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var booking = await _repository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException($"Booking with ID {id} not found");

        booking.Update(dto.Client, dto.Date);
        await _repository.UpdateAsync(booking);
    }
    public async Task DeleteBookingAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}