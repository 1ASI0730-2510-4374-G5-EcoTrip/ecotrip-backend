using ecotrip_backend.Tourists.Application;
using ecotrip_backend.Tourists.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ecotrip_backend.Controllers;

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
    [ProducesResponseType(typeof(TouristProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterTouristDto dto)
    {
        try
        {
            var touristProfile = await _service.RegisterAsync(dto);
            return Ok(touristProfile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TouristProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var profile = await _service.GetProfileByIdAsync(id);
        if (profile == null)
            return NotFound(new { error = "Perfil de turista no encontrado" });
            
        return Ok(profile);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TouristProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProfiles()
    {
        var profiles = await _service.GetAllProfilesAsync();
        return Ok(profiles);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TouristProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateTouristProfileDto dto)
    {
        try
        {
            var updatedProfile = await _service.UpdateProfileAsync(id, dto);
            if (updatedProfile == null)
                return NotFound(new { error = "Perfil de turista no encontrado" });
                
            return Ok(updatedProfile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfile(Guid id)
    {
        var deleted = await _service.DeleteProfileAsync(id);
        if (!deleted)
            return NotFound(new { error = "Perfil de turista no encontrado" });
            
        return NoContent();
    }
}
