using ecotrip_backend.Reservations.Application.DTOs;
using ecotrip_backend.Reservations.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecotrip_backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _service;
    public BookingController(BookingService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var id = await _service.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(GetBooking), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        try
        {
            var booking = await _service.GetBookingAsync(id);
            return Ok(booking);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets all bookings
    /// </summary>
    /// <returns>List of all bookings</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookingDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _service.GetAllBookingsAsync();
        return Ok(bookings);
    }

    /// <summary>
    /// Updates an existing booking
    /// </summary>
    /// <param name="id">The booking ID</param>
    /// <param name="dto">The updated booking data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] CreateBookingDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.UpdateBookingAsync(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a booking
    /// </summary>
    /// <param name="id">The booking ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        await _service.DeleteBookingAsync(id);
        return NoContent();
    }
}
