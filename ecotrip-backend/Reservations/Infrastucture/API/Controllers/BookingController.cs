using ecotrip_backend.Reservations.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecotrip_backend.Reservations.Infrastructure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _service;

        public BookingsController(BookingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking<TCreateBookingDto>([FromBody] TCreateBookingDto dto)
        {
            var id = await _service.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(GetBooking), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(Guid id)
        {
            var booking = await _service.GetBookingAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
    }
}