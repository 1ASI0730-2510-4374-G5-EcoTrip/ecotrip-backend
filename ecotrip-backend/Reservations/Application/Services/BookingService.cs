using ecotrip_backend.Reservations.Application.DTOs;
using ecotrip_backend.Reservations.Domain.Aggregates;
using ecotrip_backend.Reservations.Domain.Repositories;


namespace Reservations.Application.Services
{
    public class BookingService
    {
        private readonly BookingRepository.IBookingRepository _repository;

        public BookingService(BookingRepository.IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateBookingAsync(CreateBookingDTo dto)
        {
            var booking = new Booking(Guid.NewGuid(), dto.Client, dto.Date);
            await _repository.CreateAsync(booking);
            return booking.Id;
        }

        public async Task<BookingDTo> GetBookingAsync(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);
            return new BookingDTo
            {
                Id = booking.Id,
                Client = booking.Client,
                Date = booking.Date
            };
        }
        public async Task<object> CreateBookingAsync<TCreateBookingDto>(TCreateBookingDto dto)
        {
            throw new NotImplementedException();
        }

       
    }

   
}