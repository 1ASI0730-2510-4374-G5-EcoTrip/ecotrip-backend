using backendPrueva.Tourists.Application;
using backendPrueva.Tourists.Application.DTos;
using Microsoft.AspNetCore.Mvc;

namespace backendPrueva.Tourists.Interfaces.Controllers;

[ApiController]
[Route("api/tourists")]
public class TouristsController : ControllerBase
{
    private readonly TouristService _service;

    public TouristsController(TouristService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterTouristDto dto)
    {
        try
        {
            var tourist = await _service.RegisterAsync(dto);
            return Ok(tourist);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}